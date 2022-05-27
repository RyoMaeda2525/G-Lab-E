using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SlimeSoundEmitter : MonoBehaviour
{
    CriAtomSource _AtomSource = default;

    SlimeController _Controller = default;

    [SerializeField, Tooltip("音声名 : スライムが歩く音")]
    string _SoundNameWalk = "SE_slimeWalk";

    [SerializeField, Tooltip("音声名 : スライムがジャンプする音")]
    string _SoundNameJump = "SE_slimeJanp";

    [SerializeField, Tooltip("音声名 : スライムが草地を歩く音")]
    string _SoundNameWalkOnGrass = "SE_slimeSpringWalk";

    [SerializeField, Tooltip("音声名 : スライムが雪の上を歩く音")]
    string _SoundNameWalkOnSnow = "SE_slimeWinterWalk";

    [SerializeField, Tooltip("タグ名 : 草地")]
    string _TagNameOnGrass = "Grass";

    [SerializeField, Tooltip("タグ名 : 雪の上")]
    string _TagNameOnSnow = "Snow";

    /// <summary>true : １フレーム前は着地していた</summary>
    bool _IsGroundBefore = false;


    // Start is called before the first frame update
    void Start()
    {
        _AtomSource = GetComponentInChildren<CriAtomSource>();
        _Controller = GetComponentInChildren<SlimeController>();
        _AtomSource.loop = false;
    }

    void Update()
    {
        //再生してほしい音声
        string playOrder = null;

        //ジャンプ音
        if (_Controller.IsJump)
        {
            playOrder = _SoundNameJump;
        }
        //歩く音
        else if (_Controller.IsFoundGround && _Controller.VelocityOnPlane.sqrMagnitude > 0f)
        {
            //着地音
            if (!_IsGroundBefore)
            {
                playOrder = _SoundNameJump;
            }
            else
            {
                if (_Controller.GroundTag == _TagNameOnGrass)
                {
                    playOrder = _SoundNameWalkOnGrass;
                }
                else if (_Controller.GroundTag == _TagNameOnSnow)
                {
                    playOrder = _SoundNameWalkOnSnow;
                }
                else playOrder = _SoundNameWalk;
            }
        }

        _IsGroundBefore = _Controller.IsFoundGround;

        //ジャンプ
        if(playOrder == _SoundNameJump)
        {
            _AtomSource.Stop();
            _AtomSource.cueName = playOrder;
            _AtomSource.Play();
        }
        //ジャンプが再生対象である
        else if (_AtomSource.cueName == _SoundNameJump)
        {
            //再生が完了した
            if(_AtomSource.status == CriAtomSource.Status.PlayEnd)
            {
                _AtomSource.Stop();
                _AtomSource.cueName = null;
            }
        }
        //再生中のものが要求の音声と異なるか、再生中でない
        else if (playOrder != _SoundNameJump && (_AtomSource.cueName != playOrder || _AtomSource.status != CriAtomSource.Status.Playing))
        {
            //歩く音を出す指示がある
            if(playOrder != null)
            {
                _AtomSource.Stop();
                _AtomSource.cueName = playOrder;
                _AtomSource.Play();
            }
            //歩く音を出す指示がない
            else
            {
                _AtomSource.Stop();
                _AtomSource.cueName = null;
            }
        }
    }
}
