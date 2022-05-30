using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriSoundPlay : MonoBehaviour
{
    CriAtomSource _atomSrc;

    [SerializeField, Tooltip("soundSourceが入っているオブジェクトを選択する")]
    GameObject soundSrcObject = default;

    [SerializeField , Tooltip("サウンド名登録するためのもの")]
    string[] soundArray = default;

    [SerializeField, Tooltip("1秒で減衰するサウンドの音量")]
    float _fadeVolumeSpeed = 0.1f;

    [SerializeField, Tooltip("減衰する音量")]
    float _fadeVoume = 0;

    [SerializeField]
    bool _fadeIn = false;

    float _soundVolume = 0;

    // Start is called before the first frame update
    void Start()
    {
        _atomSrc = soundSrcObject.GetComponent<CriAtomSource>();

        _soundVolume = _atomSrc.volume;
    }

    private void Update()
    {
        if (_fadeIn && _atomSrc.volume > _fadeVoume)
        {
            _soundVolume -= Time.deltaTime * _fadeVolumeSpeed;
            _atomSrc.volume = _soundVolume;
        }
        else _fadeIn = false;
    }

    public void SoundPlay(int a) 
    {
        CriAtomSource.Status status = _atomSrc.status;

       // if (status != CriAtomSource.Status.Playing) 
       // {
            _atomSrc.Play(soundArray[a]);
       // }
       // else 
        if (status == CriAtomSource.Status.Prep)
        {
            Debug.Log($"{_atomSrc.cueSheet} {_atomSrc.cueName}");
            Debug.Log("再生準備中");
       }
    }

    public void PlayAndStopSound()
    {
        if (_atomSrc != null)
        {
            CriAtomSource.Status status = _atomSrc.status; //CriAtomSource の状態を取得
            if ((status == CriAtomSource.Status.Stop) || (status == CriAtomSource.Status.PlayEnd))
            {
                _atomSrc.Play(); //停止状態なので再生
            }
            else
            {
                _atomSrc.Stop(); //再生中なので停止
            }
        }
    }

    public void FadeInSound(float a) 
    {
        _fadeVoume = a;
        _fadeIn = true;
    }

}
