using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFloor : MonoBehaviour
{
    [SerializeField] PhysicMaterial _defaultMaterial, _iceMaterial;
    float _iceflote = 1f, _defaultflote = 2f;
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
