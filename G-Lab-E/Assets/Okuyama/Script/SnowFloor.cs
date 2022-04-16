using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFloor : MonoBehaviour
{
    /// <summary>デフォルトのdrag値</summary>
    float _defoltDrag = 2.0f;
    /// <summary>雪床のdrag値</summary>
    [SerializeField] float _snowDrag = 5.0f;
    Rigidbody _rb;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _rb = other.gameObject.GetComponent<Rigidbody>();
            _rb.drag = _snowDrag;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _rb = other.gameObject.GetComponent<Rigidbody>();
            _rb.drag = _defoltDrag;
        }
    }
}
