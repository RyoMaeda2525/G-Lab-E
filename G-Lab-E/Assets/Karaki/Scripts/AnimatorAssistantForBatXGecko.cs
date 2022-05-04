using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BatXGeckoController))]
public class AnimatorAssistantForBatXGecko : MonoBehaviour
{
    /// <summary>対象のAnimator</summary>
    Animator _Animator = default;

    /// <summary>対象のController</summary>
    BatXGeckoController _Controller = default;

    [SerializeField, Tooltip("Animatorに渡すパラメーター名 : 移動速度")]
    string _ParamNameSpeed = "Speed";

    [SerializeField, Tooltip("Animatorに渡すパラメーター名 : 滑空中か")]
    string _ParamNameIsGliding = "IsGliding";

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponent<Animator>();
        _Controller = GetComponent<BatXGeckoController>();
    }

    // Update is called once per frame
    void Update()
    {
        _Animator.SetFloat(_ParamNameSpeed, _Controller.MoveSpeed);
        _Animator.SetBool(_ParamNameIsGliding, _Controller.IsGliding);
    }
}
