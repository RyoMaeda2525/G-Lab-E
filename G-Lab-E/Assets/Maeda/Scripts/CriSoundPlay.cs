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

    // Start is called before the first frame update
    void Start()
    {
        _atomSrc = soundSrcObject.GetComponent<CriAtomSource>();
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

}
