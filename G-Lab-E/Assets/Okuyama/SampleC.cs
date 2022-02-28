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
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        _dir = new Vector3(h, 0, v);
        _dir = Camera.main.transform.TransformDirection(_dir);
        _dir.y = 0;

        
        Vector3 dir = _rb.velocity;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            this.transform.forward = dir;
        }
    }
    void FixedUpdate()
    {
        _rb.AddForce(_dir.normalized * speed);
    }
}
