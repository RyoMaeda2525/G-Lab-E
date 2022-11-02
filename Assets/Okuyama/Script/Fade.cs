using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    /// <summary>フェードアウトのImage</summary>
    Image _fadeOut;
    /// <summary>フェードアウトのスピード</summary>
    float _fadeSpeed = 0.001f;
    /// <summary>フェードアウト開始までの時間</summary>
    [SerializeField] float _startFade = 10.0f;
    /// <summary>Imageの不透明度</summary>
    float _fadeAlfa = 0;

    void Start()
    {
        _fadeOut = GetComponent<Image>();
        _fadeAlfa = _fadeOut.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        FadeOut();
    }
    void FadeOut()
    {
        _fadeOut.gameObject.SetActive(true);
        _fadeAlfa += _fadeSpeed;
        _fadeOut.color = new Color(0, 0, 0, _fadeAlfa);
    }
}
