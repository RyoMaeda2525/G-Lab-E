﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(DolphinXPenguinController))]
public class AnimatorAssistantForDolphinXPenguin : MonoBehaviour
{
    /// <summary>対象のAnimator</summary>
    Animator _Animator = default;

    /// <summary>対象のController</summary>
    DolphinXPenguinController _Controller = default;

    /// <summary>エコーロケーションを行うコンポーネント</summary>
    EcholocationController1 _Echo = default;

    [SerializeField, Tooltip("Animatorに渡すパラメーター名 : 移動速度")]
    string _ParamNameSpeed = "Speed";

    [SerializeField, Tooltip("Animatorに渡すパラメーター名 : 泳いでいる状態か")]
    string _ParamNameIsSwimming = "IsSwimming";

    [SerializeField, Tooltip("Animatorに渡すパラメーター名 : エコーロケーションを行う")]
    string _ParamNameDoEcho = "DoEcho";

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponent<Animator>();
        _Controller = GetComponent<DolphinXPenguinController>();
        _Echo = GetComponent<EcholocationController1>();
    }

    // Update is called once per frame
    void Update()
    {
        _Animator.SetFloat(_ParamNameSpeed, _Controller.MoveSpeed);
        _Animator.SetBool(_ParamNameIsSwimming, _Controller.IsSwimming);
        _Animator.SetBool(_ParamNameDoEcho, _Echo.DoneEcho);
    }
}