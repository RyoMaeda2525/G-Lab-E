using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDoor : MonoBehaviour
{
    [SerializeField] GameObject _lastDoor;
    [SerializeField] float _animStartTime = 5;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(AnimStart());
        }
    }

    IEnumerator AnimStart()
    {
        yield return new WaitForSeconds(_animStartTime);
        _lastDoor.SetActive(true);
    }
}
