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
    /// <summary></summary>
    float _animStartTime = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = _setPos.transform.position;
            _bGM.SetActive(true);
            _playerController.Invoke();
            StartCoroutine(AnimStart());
        }
    }
    IEnumerator AnimStart()
    {
        yield return new WaitForSeconds(_animStartTime);
        _anim.Play("FadeOut");
    }
}
