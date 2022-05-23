using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Elevetr : MonoBehaviour
{
    /// <summary>プレイヤーをエレベーターの真ん中に</summary>
    [SerializeField] GameObject _setPos;
    /// <summary>BGMがあるオブジェクト</summary>
    [SerializeField] GameObject _bGM;
    /// <summary>プレイヤーのコントローラーを使用不可にする</summary>
    [SerializeField] UnityEvent _playerController;
    /// <summary>フェードアウト用アニメーション</summary>
    [SerializeField] Animator _anim;
    /// <summary>フェードアウト開始までの時間</summary>
    float _animStartTime = 7.0f;
    /// <summary>ゴールした時のフラグ</summary>
    bool _goleFrag;
    GameObject _player;

    private void Update()
    {
        if(_goleFrag == true)
        {
            var _pos = _player.gameObject.transform.position;
            _player.transform.position = new Vector3(_pos.x,_pos.y + 0.035f, _pos.z) ;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = _setPos.transform.position;
            _bGM.SetActive(true);
            _playerController.Invoke();
            StartCoroutine(AnimStart());
            _goleFrag = true;
            _player = other.gameObject;
        }
    }
    IEnumerator AnimStart()
    {
        yield return new WaitForSeconds(_animStartTime);
        _anim.Play("FadeOut");
    }
}
