using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphEffectController : MonoBehaviour
{
    [SerializeField, Tooltip("スライムの液状化動作をするためのアニメーター")]
    Animator _AnimatorMorphSlimeMelting = default;

    [SerializeField, Tooltip("ヤモリ×コウモリの拡縮変身動作をするためのアニメーター")]
    Animator _AnimatorMorphBatXGeckoScaleing = default;

    [SerializeField, Tooltip("イルカ×ペンギンの拡縮変身動作をするためのアニメーター")]
    Animator _AnimatorMorphDolphinXPenguinScaleing = default;

    [SerializeField, Tooltip("スライムが液状になるアニメーションの名前")]
    string _AnimNameSlimeMelt = "Melt";

    [SerializeField, Tooltip("液状からスライムに復帰するアニメーションの名前")]
    string _AnimNameSlimeSolidification = "Solidification";

    [SerializeField, Tooltip("液状スライムを表示して非表示するアニメーションの名前")]
    string _AnimNameSlimeAppealLiquid = "AppealLiquid";

    [SerializeField, Tooltip("縮小するアニメーションの名前")]
    string _AnimNameShrinking = "Shrinking";

    [SerializeField, Tooltip("元の大きさに戻すアニメーションの名前")]
    string _AnimNameSizeReturn = "SizeReturn";

    /// <summary>true : 変身用アニメーションが再生中</summary>
    bool _IsPlayingAnimation = false;

    /// <summary>エフェクト表示時間</summary>
    float _Time = 1f;

    /// <summary>_Timeカウント用タイマー</summary>
    float _Timer = 0f;



    /// <summary>変身を終えた後に変身先のプレイヤーをActiveにするメソッド</summary>
    public Action<bool> ForEndMorphing = default;


    /// <summary>true : 変身用アニメーションが再生中</summary>
    public bool IsPlayingAnimation { get => _IsPlayingAnimation; }


    // Start is called before the first frame update
    void Start()
    {
        _Timer = 0f;
        _IsPlayingAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {
        //再生中でなければカウントしない
        if (!_IsPlayingAnimation) return;

        if (!PauseManager.IsPausing)
        {
            _Timer += Time.deltaTime;
            if (_Timer > _Time)
            {
                ForEndMorphing(true);
                ForEndMorphing = null;
                _IsPlayingAnimation = false;
                _Timer = 0f;
            }
        }
    }

    /// <summary>変身エフェクトを表示</summary>
    /// <param name="current">変身元</param>
    /// <param name="target">変身先</param>
    public void PlayMorphEffect(KindOfMorph current, KindOfMorph target)
    {
        _IsPlayingAnimation = true;

        //変身元のアニメーションを再生
        switch (current)
        {
            case KindOfMorph.Slime:
                if (_AnimatorMorphSlimeMelting) _AnimatorMorphSlimeMelting.Play(_AnimNameSlimeMelt);
                break;
            case KindOfMorph.BatXGecko:
                if (_AnimatorMorphBatXGeckoScaleing) _AnimatorMorphBatXGeckoScaleing.Play(_AnimNameShrinking);
                break;
            case KindOfMorph.DolphinXPenguin:
                if (_AnimatorMorphDolphinXPenguinScaleing) _AnimatorMorphDolphinXPenguinScaleing.Play(_AnimNameShrinking);
                break;
            default: break;
        }

        //変身先のアニメーションを再生
        switch (target)
        {
            case KindOfMorph.Slime:
                if (_AnimatorMorphSlimeMelting) _AnimatorMorphSlimeMelting.Play(_AnimNameSlimeSolidification);
                break;
            case KindOfMorph.BatXGecko:
                if (_AnimatorMorphBatXGeckoScaleing) _AnimatorMorphBatXGeckoScaleing.Play(_AnimNameSizeReturn);
                //変身元がスライムでなければ、液状化したスライムを表示するアニメーションも流す
                if (current > KindOfMorph.Slime)
                {
                    if (_AnimatorMorphSlimeMelting) _AnimatorMorphSlimeMelting.Play(_AnimNameSlimeAppealLiquid);
                }
                break;
            case KindOfMorph.DolphinXPenguin:
                if (_AnimatorMorphDolphinXPenguinScaleing) _AnimatorMorphDolphinXPenguinScaleing.Play(_AnimNameSizeReturn);
                //変身元がスライムでなければ、液状化したスライムを表示するアニメーションも流す
                if (current > KindOfMorph.Slime)
                {
                    if (_AnimatorMorphSlimeMelting) _AnimatorMorphSlimeMelting.Play(_AnimNameSlimeAppealLiquid);
                }
                break;
            default: break;
        }
    }
}
