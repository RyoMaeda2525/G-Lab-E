﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatXGeckoController : SlimeController
{
    /// <summary>重力および壁張り付き加速度</summary>
    const float GRAVITY_SPEED = 9.8f;

    /// <summary> 発見した壁の法線ベクトル </summary>
    Vector3 _FoundWallNormal = default;

    [SerializeField, Tooltip("壁のぼり可能な壁のタグ名")]
    string _TagWalkableWall = "WalkableWall";

    [SerializeField, Tooltip("地面を探す時にRayを飛ばす位置にかけるオフセット")]
    float _FindWallOffset = 0.1f;

    [SerializeField, Tooltip("坂道と認識できる角度の限界")]
    float _SlopeLimit = 40f;

    [SerializeField, Tooltip("滑空時の最低落下速度倍率")]
    float _GlideFallSpeedRate = 0.1f;

    [SerializeField, Tooltip("滑空時の落下速度が最低値になる飛行速度")]
    float _GlideFallSpeedBorder = 3f;

    /// <summary>重力速度</summary>
    float _CurrentGravitySpeed = 9.8f;

    /// <summary> 移動用メソッド </summary>
    Action Move = default;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Move = MoveWall;
        _Rb.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        if (PauseManager.IsPausing) return;

        Morphing();

        //ジャンプ入力で壁張り付き、解除
        if (InputUtility.GetDownJump)
        {
            //正面に壁や床を見つけている
            if (_FoundWallNormal.sqrMagnitude > 0)
            {
                _PlaneNormal = _FoundWallNormal;
            }
            //見つけてない
            else
            {
                _PlaneNormal = Vector3.up;
                Move = MoveGlide;
            }
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
        Vector3 forceForPb = (horizontal * right + vertical * forward) * _CurrentSpeed * 0.5f;

        _Rb.AddForce(forceForPb + (-_PlaneNormal * _CurrentGravitySpeed));
        CharacterRotation(forceForPb, _PlaneNormal, 360f);

        //落下速度調整
        float glideSpeed = Vector3.ProjectOnPlane(_Rb.velocity, _PlaneNormal).magnitude;
        float gravitySpeed = -((((_GlideFallSpeedRate - 1f) * 5f / (_GlideFallSpeedBorder)) * glideSpeed) + 8f);
        _Rb.velocity = new Vector3(_Rb.velocity.x, gravitySpeed, _Rb.velocity.z);

        //壁または床を足元から探す
        Vector3 offset = transform.forward * _FindWallOffset;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -_PlaneNormal - offset, out hit, 0.1f, _LayerGround))
        {
            _PlaneNormal = Vector3.up;
            Move = MoveWall;
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
        Vector3 forceForPb = (horizontal * right + vertical * forward) * _CurrentSpeed;
        _Rb.AddForce(forceForPb - _PlaneNormal * _CurrentGravitySpeed);
        CharacterRotation(forceForPb, _PlaneNormal, 360f);

        //壁または床を足元から探す
        Vector3 offset = transform.forward * _FindWallOffset;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -_PlaneNormal - offset, out hit, 1f, _LayerGround))
        {
            _PlaneNormal = hit.normal;
        }
        else
        {
            _PlaneNormal = Vector3.up;
            Move = MoveGlide;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        //壁のぼりできる壁である
        if (other.CompareTag(_TagWalkableWall))
        {
            _FoundWallNormal = Vector3.zero;
            //キャラクター正面の壁を見る
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.up * 0.1f, transform.forward, out hit, 1f, _LayerGround))
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
