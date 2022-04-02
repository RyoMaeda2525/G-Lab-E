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

    [SerializeField, Tooltip("true : 変身可能である")]
    protected bool _IsAbleToMorph = true;

    /// <summary> 移動用Rigidbody </summary>
    protected Rigidbody _Rb = default;

    [SerializeField, Tooltip("このキャラクターの変身の種類")]
    protected KindOfMorph _ThisMorph = KindOfMorph.Slime;

    [SerializeField, Tooltip("メインカメラの位置・向き情報")]
    protected Transform _CameraTransform = default;

    [SerializeField, Tooltip("キャラクターの移動力")]
    protected float _MoveSpeed = 30f;

    [SerializeField, Tooltip("キャラクターのジャンプ力")]
    float _JumpPower = 10f;

    [SerializeField, Tooltip("水中として認識するオブジェクトレイヤー名")]
    protected string _LayerNameWater = "Water";

    [SerializeField, Tooltip("地形として認識するオブジェクトレイヤー")]
    protected LayerMask _LayerGround = default;

    /// <summary> 移動方向平面の法線ベクトル </summary>
    protected Vector3 _PlaneNormal = new Vector3(0f, 1f, 0f);

    /// <summary> 現在の移動力 </summary>
    protected float _CurrentSpeed = 0f;

    /// <summary>地面を見つけている</summary>
    bool _IsFoundGround = false;

    [SerializeField, Tooltip("金網として認識するオブジェクトレイヤー名")]
    string _LayerNameWiremeshWall = "WireMeshWall";

    [SerializeField, Tooltip("金網をすり抜けるのにかかる時間")]
    float _ThroughWiremeshTime = 0.5f;

    /// <summary>金網すり抜けの経過時間</summary>
    float _ThroughWiremeshTimer = 0f;

    /// <summary>金網面の法線</summary>
    Vector3 _WiremeshNormal = Vector3.zero;

    #region プロパティ
    /// <summary> true : 変身可能である </summary>
    public bool IsAbleToMorph { set => _IsAbleToMorph = value; }

    /// <summary> このキャラクターの変身の種類 </summary>
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

        _CurrentSpeed = _MoveSpeed;
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

    

    void Update()
    {
        if (PauseManager.IsPausing) return;

        Morphing();

        //床を足元から探す
        _IsFoundGround = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.45f, _LayerGround))
        {
            _IsFoundGround = true;
        }

        //ジャンプ入力
        if (InputUtility.GetDownJump)
        {
            //床を見つけている
            if (_IsFoundGround)
            {
                _Rb.AddForce(Vector3.up * _JumpPower, ForceMode.Impulse);
            }
        }

        //ジャンプ力減衰
        if (!InputUtility.GetJump)
        {
            if (!_IsFoundGround && _Rb.velocity.y > 0)
            {
                _Rb.velocity = Vector3.ProjectOnPlane(_Rb.velocity, Vector3.up);
            }
        }
    }

    /// <summary> 地面を移動する </summary>
    void MoveGround()
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
        Vector3 forceForPb = (horizontal * right + vertical * forward) * _CurrentSpeed;

        //指定時間金網に触れ続けたら、突っ込む
        if (_ThroughWiremeshTimer > _ThroughWiremeshTime)
        {
            _Rb.AddForce(_WiremeshNormal * _CurrentSpeed * -0.05f, ForceMode.Impulse);
        }
        //金網に触れているが指定時間経っていないと反発
        else if (_ThroughWiremeshTimer > 0f)
        {
            if (forceForPb.sqrMagnitude > 0f)
            {
                forceForPb *= 0.3f;
            }
            else
            {
                _ThroughWiremeshTimer = 0f;
                _Rb.AddForce(_WiremeshNormal * _CurrentSpeed * 0.05f, ForceMode.Impulse);
            }
        }

        //接地状態
        if (_IsFoundGround)
        {
            _Rb.AddForce(forceForPb);
            CharacterRotation(forceForPb, Vector3.up, 360f);
        }
        else
        {
            _Rb.AddForce(forceForPb * 0.4f);
            CharacterRotation(forceForPb, Vector3.up, 90f);
        }
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
            if (up.sqrMagnitude > 0f) charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f), up);
            else charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, charDirectionQuaternion, rotateSpeed * Time.deltaTime);
        }
    }

    /// <summary> 変身する </summary>
    protected void Morphing()
    {
        //スライムに戻る
        if (InputUtility.GetDownMorphUp)
        {
            if (_ThisMorph != KindOfMorph.Slime)
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
        if (InputUtility.GetDownMorphLeft)
        {
            if (_ThisMorph != KindOfMorph.BatXGecko)
            {
                SlimeController sc = _Controllers.Where(c => c.ThisMorph == KindOfMorph.BatXGecko).FirstOrDefault();
                if (sc && sc._IsAbleToMorph)
                {
                    sc.transform.position = transform.position;
                    sc.transform.rotation = transform.rotation;
                    sc.gameObject.SetActive(true);
                    this.gameObject.SetActive(false);
                    _Morphing = KindOfMorph.BatXGecko;
                }
            }
        }

        //イルカ×ワニに変身
        if (InputUtility.GetDownMorphRight)
        {
            if (_ThisMorph != KindOfMorph.DolphinXPenguin)
            {
                SlimeController sc = _Controllers.Where(c => c.ThisMorph == KindOfMorph.DolphinXPenguin).FirstOrDefault();
                if (sc && sc._IsAbleToMorph)
                {
                    sc.transform.position = transform.position;
                    sc.transform.rotation = transform.rotation;
                    sc.gameObject.SetActive(true);
                    this.gameObject.SetActive(false);
                    _Morphing = KindOfMorph.DolphinXPenguin;
                }
            }
        }
    }

    /// <summary> 変身する種類 </summary>
    public enum KindOfMorph : byte
    {
        Slime,
        BatXGecko,
        DolphinXPenguin,
    }



    private void OnTriggerEnter(Collider other)
    {
        //金網レイヤに触れた
        if (other.gameObject.layer == LayerMask.NameToLayer(_LayerNameWiremeshWall))
        {
            //速度減衰
            _Rb.velocity = Vector3.Project(Vector3.up, _Rb.velocity);
            //地面との摩擦を無効化
            _Rb.useGravity = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //水レイヤに触れていると溺れる
        if (other.gameObject.layer == LayerMask.NameToLayer(_LayerNameWater))
        {
            _CurrentSpeed = _MoveSpeed / 5f;
            _Rb.useGravity = false;
        }

        //金網レイヤに触れている
        if (other.gameObject.layer == LayerMask.NameToLayer(_LayerNameWiremeshWall))
        {
            //金網を探す
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, other.ClosestPoint(transform.position) - transform.position, out hit, 3f, LayerMask.GetMask(_LayerNameWiremeshWall)))
            {
                _WiremeshNormal = hit.normal;
                _ThroughWiremeshTimer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //水レイヤから脱出できると元に戻る
        if (other.gameObject.layer == LayerMask.NameToLayer(_LayerNameWater))
        {
            _CurrentSpeed = _MoveSpeed;
            _Rb.useGravity = true;
        }

        //金網レイヤから離れた
        if (other.gameObject.layer == LayerMask.NameToLayer(_LayerNameWiremeshWall))
        {
            _ThroughWiremeshTimer = 0f;

            //地面との摩擦を有効化
            _Rb.useGravity = true;
        }
    }
}
