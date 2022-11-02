using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DolphinXPenguinController), typeof(Rigidbody))]
public class AnimatorAssistantForDolphinXPenguin : MonoBehaviour
{
    /// <summary>対象のAnimator</summary>
    Animator _Animator = default;

    /// <summary>対象のController</summary>
    DolphinXPenguinController _Controller = default;

    /// <summary>対象のRigidbody</summary>
    Rigidbody _Rb = default;

    /// <summary>エコーロケーションを行うコンポーネント</summary>
    EcholocationController1 _Echo = default;

    [SerializeField, Tooltip("Animatorに渡すパラメーター名 : 移動速度")]
    string _ParamNameSpeed = "Speed";

    [SerializeField, Tooltip("Animatorに渡すパラメーター名 : 泳いでいる状態か")]
    string _ParamNameIsSwimming = "IsSwimming";

    [SerializeField, Tooltip("Animatorに渡すパラメーター名 : エコーロケーションを行う")]
    string _ParamNameDoEcho = "DoEcho";

    /// <summary>true : エコーロケーションのアニメーションを再生済み</summary>
    bool DoneAnimEcho = false;

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponentInChildren<Animator>();
        _Controller = GetComponent<DolphinXPenguinController>();
        _Rb = GetComponent<Rigidbody>();
        _Echo = GetComponent<EcholocationController1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.IsPausing) return;

        _Animator.SetFloat(_ParamNameSpeed, _Rb.velocity.sqrMagnitude);
        _Animator.SetBool(_ParamNameIsSwimming, _Controller.IsSwimming);

        if (_Echo.DoneEcho)
        {
            if (!DoneAnimEcho)
            {
                _Animator.SetTrigger(_ParamNameDoEcho);
                DoneAnimEcho = true;
            }
        }
        else DoneAnimEcho = false;
    }
}
