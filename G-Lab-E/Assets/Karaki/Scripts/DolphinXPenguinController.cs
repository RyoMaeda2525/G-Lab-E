using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinXPenguinController : SlimeController
{
    /// <summary>当たり判定Collider</summary>
    CapsuleCollider _CCol = default;

    /// <summary> 移動用メソッド </summary>
    Action Move = default;

    /// <summary>True : 泳いでいる</summary>
    public bool IsSwimming { get => Move == MoveWater; }
    /// <summary>CapsuleCast用パラメータ : point1</summary>
    Vector3 Point1 { get => transform.position + (_PlaneNormal * _CCol.radius) + _CCol.center + transform.forward * (_CCol.height / 2f); }
    /// <summary>CapsuleCast用パラメータ : point2</summary>
    Vector3 Point2 { get => transform.position + (_PlaneNormal * _CCol.radius) + _CCol.center + -transform.forward * (_CCol.height / 2f); }



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Move = MoveGround;
        _CCol = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.IsPausing) return;

        if (Move != MoveWater)
        {
            Morphing();

            //床を足元から探す
            _IsFoundGround = false;
            RaycastHit hit;
            if (Physics.CapsuleCast(Point1, Point2, _CCol.radius, -_PlaneNormal, out hit, 0.45f, _LayerGround))
            {
                _IsFoundGround = true;
            }
        }
    }

    void FixedUpdate()
    {
        Move();
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
        Vector3 forceForPb = (horizontal * right + vertical * forward) * _MoveSpeed;

        if (forceForPb.sqrMagnitude > 0f)
        {
            _Rb.AddForce(forceForPb);
            CharacterRotation(forceForPb, Vector3.up, 360f);
        }
    }

    /// <summary> 水中移動 </summary>
    void MoveWater()
    {
        //基本情報をローカルで定義
        //カメラ視点の正面(重力軸無視)
        Vector3 forward = _CameraTransform.forward;
        //カメラ視点の右方向
        Vector3 right = _CameraTransform.right;

        //プレイヤーの移動入力を取得
        float horizontal = InputUtility.GetAxis2DMove.x;
        float vertical = InputUtility.GetAxis2DMove.y;

        //プレーヤーを移動させることができる状態なら、移動させたい度合・方向を取得
        Vector3 forceForPb = (horizontal * right + vertical * forward) * _MoveSpeed;

        if(forceForPb.sqrMagnitude > 0f)
        {
            _Rb.AddForce(forceForPb);
            CharacterRotation(forceForPb, Vector3.zero, 360f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //水レイヤに触れているときに泳ぐ
        if(other.gameObject.layer == LayerMask.NameToLayer(_LayerNameWater))
        {
            Move = MoveWater;
            _Rb.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_LayerNameWater))
        {
            Move = MoveGround;
            CharacterRotation(transform.forward, Vector3.up, 360f);
            _Rb.useGravity = true;
        }
    }
}
