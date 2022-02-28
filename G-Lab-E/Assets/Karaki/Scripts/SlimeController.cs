using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SlimeController : MonoBehaviour
{
    /// <summary> 操作対象になりうるコンポーネント群 </summary>
    static List<SlimeController> _Controllers = new List<SlimeController>();

    /// <summary> 現在の変身先 </summary>
    static KindOfMorph _Morphing = KindOfMorph.Slime;

    /// <summary> 移動用Rigidbody </summary>
    protected Rigidbody _Rb = default;

    [SerializeField, Tooltip("このキャラクターの変身の種類")]
    protected KindOfMorph _ThisMorph = KindOfMorph.Slime;

    [SerializeField, Tooltip("メインカメラの位置・向き情報")]
    protected Transform _CameraTransform = default;

    [SerializeField, Tooltip("キャラクターの移動力")]
    protected float _MoveSpeed = 30f;

    /// <summary> 移動方向平面の法線ベクトル </summary>
    protected Vector3 _PlaneNormal = new Vector3(0f, 1f, 0f);

    #region プロパティ
    public KindOfMorph ThisMorph { get => _ThisMorph; }
    #endregion

    protected void Awake()
    {
        _Controllers.Add(this);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Rb = GetComponent<Rigidbody>();

        //現在の変身先のモノだけ有効化
        this.gameObject.SetActive(_Morphing == _ThisMorph);
    }

    protected void OnDestroy()
    {
        _Controllers.Remove(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveGround();
    }

    protected virtual void Update()
    {
        Morphing();
    }

    /// <summary> 地面を移動する </summary>
    protected virtual void MoveGround()
    {
        //基本情報をローカルで定義
        //カメラ視点の正面(重力軸無視)
        Vector3 forward = Vector3.ProjectOnPlane(_CameraTransform.forward, _PlaneNormal);
        forward = forward.normalized;
        //カメラ視点の右方向
        Vector3 right = Vector3.ProjectOnPlane(_CameraTransform.right, _PlaneNormal);
        right = right.normalized;

        //プレイヤーの移動入力を取得
        float horizontal = InputUtility.GetAxis2DMove.x;
        float vertical = InputUtility.GetAxis2DMove.y;

        //プレーヤーを移動させることができる状態なら、移動させたい度合・方向を取得
        Vector3 forceForPb = (horizontal * right + vertical * forward) * _MoveSpeed;


        _Rb.AddForce(forceForPb);
        CharacterRotation(forceForPb, Vector3.up, 360f);
    }

    /// <summary> キャラクターを指定向きに回転させる </summary>
    /// <param name="targetDirection">目標向き</param>
    /// <param name="up">上方向（Vector.Zeroなら上方向を指定しない）</param>
    /// <param name="rotateSpeed">回転速度</param>
    protected void CharacterRotation(Vector3 targetDirection, Vector3 up, float rotateSpeed)
    {
        if (targetDirection.sqrMagnitude > 0.0f)
        {
            Vector3 trunDirection = transform.right;
            Quaternion charDirectionQuaternion = Quaternion.identity;
            if(up.sqrMagnitude > 0f) charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f), up);
            else charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, charDirectionQuaternion, rotateSpeed * Time.deltaTime);
        }
    }

    /// <summary> 変身する </summary>
    protected void Morphing()
    {
        //スライムに戻る
        if (InputUtility.GetDownMorphDown)
        {
            if(_ThisMorph != KindOfMorph.Slime)
            {
                SlimeController sc = _Controllers.Where(c => c.ThisMorph == KindOfMorph.Slime).First();
                sc.transform.position = transform.position;
                sc.transform.rotation = transform.rotation;
                sc.gameObject.SetActive(true);
                this.gameObject.SetActive(false);
                _Morphing = KindOfMorph.Slime;
            }
        }

        //コウモリ×ヤモリに変身
        if (InputUtility.GetDownMorphUp)
        {
            if (_ThisMorph != KindOfMorph.BatXGecko)
            {
                SlimeController sc = _Controllers.Where(c => c.ThisMorph == KindOfMorph.BatXGecko).First();
                sc.transform.position = transform.position;
                sc.transform.rotation = transform.rotation;
                sc.gameObject.SetActive(true);
                this.gameObject.SetActive(false);
                _Morphing = KindOfMorph.BatXGecko;
            }
        }

        //イルカ×ワニに変身
        if (InputUtility.GetDownMorphRight)
        {
            if(_ThisMorph != KindOfMorph.DolphinXCrocodile)
            {
                SlimeController sc = _Controllers.Where(c => c.ThisMorph == KindOfMorph.DolphinXCrocodile).First();
                sc.transform.position = transform.position;
                sc.transform.rotation = transform.rotation;
                sc.gameObject.SetActive(true);
                this.gameObject.SetActive(false);
                _Morphing = KindOfMorph.DolphinXCrocodile;
            }
        }
    }

    /// <summary> 変身する種類 </summary>
    public enum KindOfMorph : byte
    {
        Slime,
        BatXGecko,
        DolphinXCrocodile,
    }
}
