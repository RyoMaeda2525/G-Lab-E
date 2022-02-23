using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SlimeController : MonoBehaviour
{
    /// <summary> 移動用Rigidbody </summary>
    protected Rigidbody _Rb = default;

    [SerializeField, Tooltip("メインカメラの位置・向き情報")]
    protected Transform _CamaeraTransform = default;

    [SerializeField, Tooltip("移動方向平面の法線ベクトル")]
    protected Vector3 _PlaneNormal = new Vector3(0f, 1f, 0f);

    [SerializeField, Tooltip("キャラクターの移動力")]
    protected float _MoveSpeed = 30f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveGround();
    }

    /// <summary> 地面を移動する </summary>
    protected virtual void MoveGround()
    {
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
        Vector3 forceForPb = (horizontal * right + vertical * forward) * _MoveSpeed;


        _Rb.AddForce(forceForPb);
        CharacterRotation(forceForPb, Vector3.up, 360f);
    }

    /// <summary> キャラクターを指定向きに回転させる </summary>
    /// <param name="targetDirection">目標向き</param>
    /// <param name="rotateSpeed">回転速度</param>
    protected void CharacterRotation(Vector3 targetDirection, Vector3 up, float rotateSpeed)
    {
        if (targetDirection.sqrMagnitude > 0.0f)
        {
            Vector3 trunDirection = transform.right;
            Quaternion charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f), up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, charDirectionQuaternion, rotateSpeed * Time.deltaTime);
        }
    }
}
