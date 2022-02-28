using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movewithfloor : MonoBehaviour
{
    public Vector3 defaultScale = Vector3.zero;
    void Start()
    {
        defaultScale = transform.lossyScale;
    }

    void Update()
    {
        Vector3 lossScale = transform.lossyScale;
        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(
                localScale.x / lossScale.x * defaultScale.x,
                localScale.y / lossScale.y * defaultScale.y,
                localScale.z / lossScale.z * defaultScale.z
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        if (transform.parent == null && collision.gameObject.tag == "MoveFloor")
        {
            var emptyObject = new GameObject();
            emptyObject.transform.parent = collision.gameObject.transform;
            transform.parent = emptyObject.transform;
        }

    }

    void OnCollisionExit(Collision collision)
    {
        if (transform.parent != null && collision.gameObject.tag == "MoveFloor")
        {
            transform.parent = null;
        }
    }
}
