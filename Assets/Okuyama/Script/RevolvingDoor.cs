using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingDoor : MonoBehaviour
{
    /// <summary>180度に戻すまでの時間</summary>
    [SerializeField] float _exitTime = 2;
    Rigidbody _rb;

    private void OnCollisionExit(Collision collision)
    {
        StartCoroutine("Reset");
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(_exitTime);
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        this.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
