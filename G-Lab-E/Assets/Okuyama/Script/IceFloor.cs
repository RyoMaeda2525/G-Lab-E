using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFloor : MonoBehaviour
{
    /// <summary>デフォルトのPhysicMaterial</summary>
    [SerializeField] PhysicMaterial _defaultMaterial;
    /// <summary>滑る床のPhysicMaterial</summary>
    [SerializeField] PhysicMaterial _iceMaterial;
    /// <summary>デフォルトのdrag値</summary>
    float _defaultflote = 2f;
    /// <summary>滑る床のdrag値</summary>
    [SerializeField] float _iceflote = 1f;
    Rigidbody _rb;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.material = _iceMaterial;
            _rb = collider.gameObject.GetComponent<Rigidbody>();
            _rb.drag = _iceflote;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.material = _defaultMaterial;
            _rb = collider.gameObject.GetComponent<Rigidbody>();
            _rb.drag = _defaultflote;
        }
    }
}
