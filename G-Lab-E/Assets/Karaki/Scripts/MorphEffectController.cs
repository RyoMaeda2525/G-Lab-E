using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphEffectController : MonoBehaviour
{
    [SerializeField, Tooltip("エフェクト表示時間")]
    float _Time = 1f;

    /// <summary>_Timeカウント用タイマー</summary>
    float _Timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        _Timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseManager.IsPausing)
        {
            _Timer += Time.deltaTime;
            if (_Timer > _Time)
            {
                Destroy(gameObject);
            }
        }
    }
}
