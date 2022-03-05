using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleC : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody _rb;
    Vector3 _dir;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        _rb.AddForce(_dir.normalized * speed);
    }
}
