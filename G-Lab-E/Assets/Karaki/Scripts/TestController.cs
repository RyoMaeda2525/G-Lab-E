using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TestController : MonoBehaviour
{
    Rigidbody _Rb = default;

    [SerializeField, Tooltip("メインカメラの位置・向き情報")]
    Transform _CamaeraTransform = default;

    [SerializeField, Tooltip("移動方向平面の法線ベクトル")]
    Vector3 _PlaneNormal = new Vector3(0f, 1f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        _Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //移動時にAddForceするVector3
        Vector3 ForceForPb = Vector3.zero;

        //基本情報をローカルで定義
        //カメラ視点の正面(重力軸無視)
        Vector3 forward = Vector3.ProjectOnPlane(_CamaeraTransform.forward, _PlaneNormal);
        forward = forward.normalized;
        //カメラ視点の右方向
        Vector3 right = Vector3.ProjectOnPlane(_CamaeraTransform.right, _PlaneNormal);
        right = right.normalized;

        //プレイヤーの移動入力を取得
        float horizontal = InputUtility.GetAxis2DMove.x;
        float vertical = InputUtility.GetAxis2DMove.y;

        //プレーヤーを移動させることができる状態なら、移動させたい度合・方向を取得
        ForceForPb = (horizontal * right + vertical * forward) * 20f;


        _Rb.AddForce(ForceForPb);
    }
}
