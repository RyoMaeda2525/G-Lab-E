using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Elevetr : MonoBehaviour
{
    /// <summary>プレイヤーをエレベーターの真ん中に</summary>
    [SerializeField] GameObject _setPos;
    /// <summary>BGMがあるオブジェクト</summary>
    [SerializeField] GameObject _bGM;
    /// <summary>プレイヤーのコントローラーを使用不可にする</summary>
    [SerializeField] UnityEvent _playerController;
    /// <summary>フェードアウトのImage</summary>
    [SerializeField] Image _fadeOut;
    /// <summary>フェードアウトのスピード</summary>
    [SerializeField] float _fadeSpeed = 0.001f;
    /// <summary>フェードアウト開始までの時間</summary>
    [SerializeField]float _startFade = 10.0f;

    /// <summary>Imageの不透明度</summary>
    float _fadeAlfa = 0;
    /// <summary>フェードアウト用Bool</summary>
    bool _fadeOutBool = false;

    private void Start()
    {
        _fadeAlfa = _fadeOut.color.a;
    }
    private void Update()
    {
        if (_fadeOutBool) FadeOut();        
    }
    void FadeOut()
    {
        _fadeOut.gameObject.SetActive(true);
        _fadeAlfa += _fadeSpeed;
        _fadeOut.color = new Color(0, 0, 0, _fadeAlfa);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = _setPos.transform.position;
            _bGM.SetActive(true);
            _playerController.Invoke();
            StartCoroutine(FadeOutC());
        }
    }
    IEnumerator FadeOutC()
    {
        yield return new WaitForSeconds(_startFade);
        _fadeOutBool = true;
    }
}
