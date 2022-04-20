using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>BeltScrollさせるRigidbody</summary>
    Rigidbody _Rb = default;

    // Start is called before the first frame update
    void Start()
    {
        _Rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
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
}
