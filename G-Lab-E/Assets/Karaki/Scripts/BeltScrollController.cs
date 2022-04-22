using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class BeltScrollController : MonoBehaviour
{
    [SerializeField, Tooltip("移動可能な範囲の最大値(長方形右上座標)")]
    Vector2 _MoveAreaMax = new Vector2(3f, 2f);

    [SerializeField, Tooltip("移動可能な範囲の最小値(長方形左下座標)")]
    Vector2 _MoveAreaMin = new Vector2(-3f, -2f);

    [SerializeField, Tooltip("上下左右の移動力")]
    float _MoveSpeed = 10f;

    [SerializeField, Tooltip("移動可能な範囲からはみ出そうな時に、押し戻しをかける力の倍率")]
    float _PushBackToAreaSpeedRatio = 1f;

    [SerializeField, Tooltip("障害物とみなすオブジェクトのレイヤー名")]
    string _LayerNameObstacle = "Ground";

    [SerializeField, Tooltip("許されるダメージ回数")]
    byte _DamageCount = 3;

    [SerializeField, Tooltip("ダメージを受けれる残り回数")]
    byte _DamageLeave = 0;

    /// <summary>BeltScrollさせるRigidbody</summary>
    Rigidbody _Rb = default;

    [SerializeField, Tooltip("暗転させるメソッドをアサイン")]
    UnityEvent _DoBlackoutMethod = default;

    [SerializeField, Tooltip("暗転から復帰するメソッドをアサイン")]
    UnityEvent _ReturnFromBlackoutMethod = default;

    [SerializeField, Tooltip("CinemachineDollyCartで、通過失敗した際に復帰する位置をセットするメソッドをアサイン")]
    UnityEvent _SetPosition = default;


    /// <summary>ダメージを受けれる残り回数</summary>
    public byte DamageLeave { get => _DamageLeave; set => _DamageLeave = value; }


    // Start is called before the first frame update
    void Start()
    {
        _Rb = GetComponent<Rigidbody>();
        _DamageLeave = _DamageCount;
    }

    void FixedUpdate()
    {
        if (_DamageLeave < 1) return;

        //キャラクターが移動範囲内にいるかどうかを確認
        bool isLowerOnAreaHorizontal = transform.localPosition.x < _MoveAreaMin.x;
        bool isUpperOnAreaHorizontal = _MoveAreaMax.x < transform.localPosition.x;
        bool isLowerOnAreaVertical = transform.localPosition.y < _MoveAreaMin.y;
        bool isUpperOnAreaVertical = _MoveAreaMax.y < transform.localPosition.y;

        //移動入力があれば移動
        if (InputUtility.GetAxis2DMove.sqrMagnitude > 0f)
        {
            //プレイヤーの移動入力を取得
            float horizontal = InputUtility.GetAxis2DMove.x;
            float vertical = InputUtility.GetAxis2DMove.y;

            //移動力指定
            if (isLowerOnAreaHorizontal || isUpperOnAreaHorizontal) horizontal = 0f;
            if (isLowerOnAreaVertical || isUpperOnAreaVertical) vertical = 0f;
            Vector3 forceForPb = (Vector3.up * vertical + Vector3.right * horizontal) * _MoveSpeed;

            //加算
            _Rb.AddRelativeForce(forceForPb);
        }

        //範囲外なら範囲内に向け力をかける
        if (isLowerOnAreaHorizontal)
        {
            _Rb.AddRelativeForce(Vector3.right * _MoveSpeed * _PushBackToAreaSpeedRatio);
        }
        else if (isUpperOnAreaHorizontal)
        {
            _Rb.AddRelativeForce(Vector3.left * _MoveSpeed * _PushBackToAreaSpeedRatio);
        }
        if (isLowerOnAreaVertical)
        {
            _Rb.AddRelativeForce(Vector3.up * _MoveSpeed * _PushBackToAreaSpeedRatio);
        }
        else if (isUpperOnAreaVertical)
        {
            _Rb.AddRelativeForce(Vector3.down * _MoveSpeed * _PushBackToAreaSpeedRatio);
        }
    }

    /// <summary>コースに復帰する流れを実施するコルーチン</summary>
    IEnumerator RecoverCoroutine()
    {
        //gameObject.SetActive(false);

        WaitForSeconds wait = new WaitForSeconds(1f);

        _DoBlackoutMethod.Invoke();

        yield return wait;

        _SetPosition.Invoke();

        yield return wait;

        _DamageLeave = _DamageCount;
        //gameObject.SetActive(true);

        _ReturnFromBlackoutMethod.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        if (_DamageLeave < 1) return;

        if (other.gameObject.layer == LayerMask.NameToLayer(_LayerNameObstacle))
        {
            _DamageLeave -= 1;

            if (_DamageLeave < 1) StartCoroutine(RecoverCoroutine());
        }
    }
}