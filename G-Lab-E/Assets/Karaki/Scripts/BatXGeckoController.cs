using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatXGeckoController : SlimeController
{
    #region メンバ
    /// <summary>当たり判定Collider</summary>
    CapsuleCollider _Ccol = default;

    /// <summary>重力および壁張り付き加速度</summary>
    const float GRAVITY_SPEED = 9.8f;

    /// <summary>滑空時の重力加速度</summary>
    const float GLIDE_GRAVITY_SPEED = 1.75f;

    /// <summary> 発見した壁の法線ベクトル </summary>
    Vector3? _FoundWallNormal = null;

    [SerializeField, Tooltip("壁のぼり可能な壁のタグ名")]
    string _TagWalkableWall = "WalkableWall";

    [SerializeField, Tooltip("地面を探す時にRayを飛ばす位置にかけるオフセット")]
    float _FindWallOffset = 0.1f;

    [SerializeField, Tooltip("坂道と認識できる角度の限界")]
    float _SlopeLimit = 40f;

    [SerializeField, Tooltip("ヤモリ×コウモリの壁を這う時の移動力")]
    float _MoveSpeedWall = 5f;

    /// <summary>重力速度</summary>
    float _CurrentGravitySpeed = 9.8f;

    /// <summary> 移動用メソッド </summary>
    Action Move = default;
    #endregion

    #region プロパティ
    /// <summary>CapsuleCast用パラメータ : point1</summary>
    Vector3 Point1 { get => transform.position + (_PlaneNormal * _Ccol.radius) + _Ccol.center + transform.forward * (_Ccol.height / 2f); }
    /// <summary>CapsuleCast用パラメータ : point2</summary>
    Vector3 Point2 { get => transform.position + (_PlaneNormal * _Ccol.radius) + _Ccol.center + -transform.forward * (_Ccol.height / 2f); }
    /// <summary>True : 滑空中である</summary>
    public bool IsGliding { get => Move == MoveGlide; }
    #endregion


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Move = MoveWall;
        _Rb.useGravity = false;
        _Ccol = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        if (PauseManager.IsPausing) return;

        _IsFoundGround = Move == MoveGround;

        Morphing();

        //ジャンプ入力で壁張り付き、解除
        if (InputUtility.GetDownJump)
        {
            //正面に壁や床を見つけている
            if (_FoundWallNormal != null)
            {
                _PlaneNormal = (Vector3)_FoundWallNormal;
                Move = MoveWall;
                CharacterRotation(transform.forward, _PlaneNormal, 360f);
                _Rb.velocity = Vector3.zero;
            }
            //見つけてない
            else
            {
                //壁張り付きの時
                if(Move == MoveWall)
                {
                    _PlaneNormal = Vector3.up;
                    Move = MoveGlide;
                    _CurrentGravitySpeed = GLIDE_GRAVITY_SPEED;
                }
                //滑空中の時
                else if(Move == MoveGlide)
                {
                    //ジャンプボタンにより滑空・落下を切り替える
                    if (InputUtility.GetDownJump)
                    {
                        if (_CurrentGravitySpeed < GRAVITY_SPEED)
                        {
                            _CurrentGravitySpeed = GRAVITY_SPEED;
                        }
                        else
                        {
                            _CurrentGravitySpeed = GLIDE_GRAVITY_SPEED;
                        }
                    }
                }
            }
        }
        _FoundWallNormal = null;
    }

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

        _Rb.AddForce(forceForPb + (-_PlaneNormal * _CurrentGravitySpeed));
        CharacterRotation(forceForPb, _PlaneNormal, 720f);

        //床を足元から探す
        Vector3 offset = transform.forward * _FindWallOffset;
        RaycastHit hit;
        if (Physics.CapsuleCast(Point1, Point2, _Ccol.radius, -_PlaneNormal, out hit, 0.65f, _LayerGround))
        {
            _PlaneNormal = Vector3.up;
        }
        else
        {
            Move = MoveGlide;
            _CurrentGravitySpeed = GLIDE_GRAVITY_SPEED;
        }
    }

    void MoveGlide()
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
        Vector3 forceForPb = (horizontal * right + vertical * forward) * _CurrentSpeed * 1.2f;

        _Rb.AddForce(forceForPb + (-_PlaneNormal * _CurrentGravitySpeed));
        CharacterRotation(forceForPb, _PlaneNormal, 360f);

        //壁または床を足元から探す
        Vector3 offset = transform.forward * _FindWallOffset;
        RaycastHit hit;
        if (Physics.CapsuleCast(Point1, Point2, _Ccol.radius, -_PlaneNormal, out hit, 0.65f, _LayerGround))
        {
            _PlaneNormal = Vector3.up;
            Move = MoveGround;
            _CurrentGravitySpeed = GRAVITY_SPEED;
        }
    }

    void MoveWall()
    {
        //基本情報をローカルで定義
        //カメラ視点の正面(重力軸無視)
        Vector3 forward = Vector3.ProjectOnPlane(_CameraTransform.up, _PlaneNormal);
        forward = forward.normalized;
        //カメラ視点の右方向
        Vector3 right = Vector3.ProjectOnPlane(_CameraTransform.right, _PlaneNormal);
        right = right.normalized;

        //プレイヤーの移動入力を取得
        float horizontal = InputUtility.GetAxis2DMove.x;
        float vertical = InputUtility.GetAxis2DMove.y;

        //プレーヤーを移動させることができる状態なら、移動させたい度合・方向を取得
        Vector3 forceForPb = (horizontal * right + vertical * forward) * _MoveSpeedWall;
        _Rb.AddForce(forceForPb - _PlaneNormal * _CurrentGravitySpeed);
        CharacterRotation(forceForPb, _PlaneNormal, 360f);
        if (_Rb.velocity.sqrMagnitude > 2f) _Rb.velocity = _Rb.velocity.normalized;

        //壁または床を足元から探す
        Vector3 offset = transform.forward * _FindWallOffset;
        RaycastHit hit;
        if (Physics.CapsuleCast(Point1, Point2, _Ccol.radius, -_PlaneNormal, out hit, 0.65f, _LayerGround)
            && (hit.collider.CompareTag(_TagWalkableWall)))
        {
            _PlaneNormal = hit.normal;
        }
        else
        {
            _PlaneNormal = Vector3.up;
            Move = MoveGlide;
            _CurrentGravitySpeed = GLIDE_GRAVITY_SPEED;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        //壁のぼりできる壁である
        if (other.CompareTag(_TagWalkableWall))
        {
            //キャラクター正面の壁を見る
            RaycastHit hit;
            Vector3 extents = new Vector3(_Ccol.radius, _Ccol.radius, 0.05f); 
            if(Physics.BoxCast(transform.position + (transform.up * 0.1f) + -(transform.forward * 0.1f), extents, transform.forward, out hit, transform.rotation, 1f, _LayerGround))
            {
                if (Vector3.Angle(hit.normal, transform.up) > _SlopeLimit)
                {
                    _FoundWallNormal = hit.normal;
                }
            }
        }

        //水レイヤに触れていると溺れる
        if (other.gameObject.layer == LayerMask.NameToLayer(_LayerNameWater))
        {
            _CurrentSpeed = _MoveSpeed * 0.1f;
            _CurrentGravitySpeed = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //水レイヤから脱出できると元に戻る
        if (other.gameObject.layer == LayerMask.NameToLayer(_LayerNameWater))
        {
            _CurrentSpeed = _MoveSpeed;
            _CurrentGravitySpeed = GRAVITY_SPEED;
        }
    }
}
