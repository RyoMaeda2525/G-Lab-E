using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicles : MonoBehaviour
{
    /// <summary>つらら用エフェクト</summary>
    //[SerializeField] GameObject　_iciclesPartical;
    /// <summary>つららオブジェクト</summary>
    [SerializeField] GameObject _iceclesObj;
    Vector3 _IceclesPos;

    void Start()
    {
        _IceclesPos = _iceclesObj.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")//プレイヤーに当たったら
        {
            //エフェクト用
            //Instantiate(_iciclesPartical, this.transform.position, Quaternion.identity);
            Destroy(this._iceclesObj);
            Instans();
        }
    }
    private void Instans()
    {
        GameObject _instansIcecles = Instantiate(_iceclesObj, _IceclesPos, Quaternion.identity);
        _instansIcecles.SetActive(false);
        _iceclesObj = _instansIcecles;
    }
}
