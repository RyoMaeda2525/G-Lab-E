using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatXGeckoController : SlimeController
{
    [SerializeField, Tooltip("地形として認識するオブジェクトレイヤー")]
    LayerMask _LayerGround = default;

    /// <summary> 発見した壁の法線ベクトル </summary>
    Vector3 _FoundWallNormal = default;

    [SerializeField, Tooltip("地面を探す時にRayを飛ばす位置にかけるオフセット")]
    float _FindWallOffset = 1f;

    [SerializeField, Tooltip("坂道と認識できる角度の限界")]
    float _SlopeLimit = 40f;

    /// <summary> 移動用メソッド </summary>
    Action Move = default;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Move = MoveGround;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    /// <summary> 移動方法を壁移動から床移動に変更する手続き </summary>
    void ChangeMoveWallToGround()
    {
        _Rb.rotation = Quaternion.LookRotation(_PlaneNormal, Vector3.up);
        _PlaneNormal = Vector3.up;
        Move = MoveGround;
        _Rb.useGravity = true;
    }

    /// <summary> 移動方法をを床移動から壁移動に変更する手続き </summary>
    void ChangeMoveGroundToWall(Vector3 wallNormal)
    {
        _PlaneNormal = wallNormal;
        _Rb.position += Vector3.up * 1f;
        _Rb.rotation = Quaternion.LookRotation(Vector3.up, _PlaneNormal);
        Move = MoveWall;
        _Rb.useGravity = false;
    }

    protected override void MoveGround()
    {
        base.MoveGround();

        if (_FoundWallNormal.sqrMagnitude > 0f)
        {
            if (InputUtility.GetDownJump)
            {
                ChangeMoveGroundToWall(_FoundWallNormal);
            }
        }
    }

    void MoveWall()
    {
        //基本情報をローカルで定義
        //カメラ視点の正面(重力軸無視)
        Vector3 forward = Vector3.ProjectOnPlane(Vector3.up, _PlaneNormal);
        forward = forward.normalized;
        //カメラ視点の右方向
        Vector3 right = Vector3.ProjectOnPlane(_CamaeraTransform.right, _PlaneNormal);
        right = right.normalized;

        //プレイヤーの移動入力を取得
        float horizontal = InputUtility.GetAxis2DMove.x;
        float vertical = InputUtility.GetAxis2DMove.y;

        //プレーヤーを移動させることができる状態なら、移動させたい度合・方向を取得
        Vector3 forceForPb = (horizontal * right + vertical * forward) * _MoveSpeed;

        _Rb.AddForce(forceForPb - transform.up * 9.8f);
        CharacterRotation(forceForPb, _PlaneNormal, 360f);

        //壁を足元から探す
        Vector3 offset = transform.forward * _FindWallOffset;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -_PlaneNormal - offset, out hit, 5f, _LayerGround))
        {
            //壁法線が鉛直とほぼ変わらないくらいなら壁移動解除
            if (Vector3.Angle(Vector3.up, hit.normal) < _SlopeLimit)
            {
                ChangeMoveWallToGround();
            }
            //そうでなければ移動方向平面の法線を更新
            else _PlaneNormal = hit.normal;
        }
        //なければ壁移動解除
        else
        {
            ChangeMoveWallToGround();
        }

        //ジャンプボタンが押されたら壁移動解除
        if (InputUtility.GetDownJump)
        {
            ChangeMoveWallToGround();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        _FoundWallNormal = Vector3.zero;
        //キャラクター正面の壁を見る
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.75f, transform.forward, out hit, 5f, _LayerGround))
        {
            if (Vector3.Angle(hit.normal, transform.up) > _SlopeLimit)
            {
                _FoundWallNormal = hit.normal;
            }
        }
    }
}
