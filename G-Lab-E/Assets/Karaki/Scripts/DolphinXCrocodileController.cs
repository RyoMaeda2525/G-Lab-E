using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinXCrocodileController : SlimeController
{
    [SerializeField, Tooltip("水中として認識するオブジェクトレイヤー名")]
    string _LayerNameWater = "Water";

    /// <summary> 移動用メソッド </summary>
    Action Move = default;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Move = MoveGround;
    }

    // Update is called once per frame
    new void Update()
    {
        if (Move != MoveWater) Morphing();
    }

    void FixedUpdate()
    {
        Move();
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
