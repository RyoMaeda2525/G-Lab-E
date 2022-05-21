using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFrag : MonoBehaviour
{
    [SerializeField,Tooltip("SkinnedMeshRenderer")] 
    SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField, Tooltip("アニメーション")]
    Animator _anim;
    [SerializeField, Tooltip("橋から落ちれるようにする")]
    GameObject _meshFloor;
    [Tooltip("霧のDensityの値")]
    float _density = 0.14f;
    [Tooltip("霧のDensityから引く値")]
    const float DENSITY_MINUS = 0.035f;
    [Tooltip("雪が減る前の値")]
    float _blendShapes = 0f;
    [Tooltip("雪が減る時に足す値")]
    const float SNOW_PULAS = 24.86f;
    

    void Start()//一応初期値のセット
    {
        RenderSettings.fogDensity = _density;
        _skinnedMeshRenderer.SetBlendShapeWeight(0, _blendShapes);
    }

    public void SnowFog()//霧と雪を薄くする
    {
        RenderSettings.fogDensity = _density -= DENSITY_MINUS;
        _skinnedMeshRenderer.SetBlendShapeWeight(0, _blendShapes += SNOW_PULAS);
    }

    public void OpenDoor()
    {
        if(_density == 0 &&_blendShapes == 99.44f)
        {
            _anim.Play("BigDoor");
            _meshFloor.SetActive(true);
        }
    }
}
