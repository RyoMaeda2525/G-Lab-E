using System.Collections;
using UnityEngine;

public class IceEffect : MonoBehaviour
{
    [SerializeField] GameObject _ice;
    [SerializeField] float _setOnIceTime = 1.2f;

    public void Start()
    {
        StartCoroutine("SetOn");
    }
    IEnumerator SetOn()
    {
        yield return new WaitForSeconds(_setOnIceTime);
        _ice.SetActive(true);
    }
}
