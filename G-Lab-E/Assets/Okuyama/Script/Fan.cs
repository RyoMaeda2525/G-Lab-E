using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    /// <summary>空気抵抗係数</summary>
    [SerializeField] float coefficient = 5.0f;
    /// <summary>風速</summary>
    [SerializeField] Vector3 velocity;
    /// <summary>プレイヤーのRigidbody</summary>
    Rigidbody _rb;

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag != "Player")
        {
            return;
        }

        _rb = collider.gameObject.GetComponent<Rigidbody>();
        // 相対速度
        var relativeVelocity = velocity - _rb.velocity;
        // 空気抵抗
        _rb.AddForce(coefficient * relativeVelocity);
    }
}
