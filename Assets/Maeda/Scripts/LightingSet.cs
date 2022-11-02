﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightingSet : MonoBehaviour
{
    [SerializeField, Tooltip("IntensityMultiplierの値")] 
    private float _Lightingintensity = 0;

    [SerializeField, Tooltip("field_helloween_Moonのオブジェクト")]
    private GameObject _moon = default;

    [SerializeField, Tooltip("field_helloween_Moonのマテリアル」")]
    private Material _moonMaterial = default;

    [SerializeField, Tooltip("IntensityMultiplierの値")]
    private float _moonIntensity = 0;

    [SerializeField, Tooltip("Light_moonlightのLight")]
    private Light _moonSpotLight = default;

    [SerializeField, Tooltip("Intensityの値")]
    private float _moonSpotIntensity = 0;

    [SerializeField, Tooltip("Light_moonlightのLight")]
    private Light _directionalLight = default;

    [SerializeField, Tooltip("Intensityの値")]
    private float _directionalLightIntensity = 0;

    [SerializeField, Tooltip("外周4つの発電機が起動した時に起こすイベント")]
    UnityEvent FullLightUp = default;

    // Start is called before the first frame update
    void Awake() //明かりの取得と暗くする部分
    {
        _moonMaterial = _moon.GetComponent<Renderer>().material;

        RenderSettings.ambientIntensity = _Lightingintensity;
        _moonMaterial.SetFloat("_EmissionIntensity", _moonIntensity);
        _moonSpotLight.intensity = _moonSpotIntensity;
        _directionalLight.intensity = _directionalLightIntensity;
    }

    public void LightUp() //呼び出す度に明るくなる 
    {
        RenderSettings.ambientIntensity = _Lightingintensity += 0.25f;
        _moonMaterial.SetFloat("_EmissionIntensity", _moonIntensity += 20);
        _moonSpotLight.intensity = _moonSpotIntensity += 2;
        _directionalLight.intensity = _directionalLightIntensity += 0.25f;
    }

    public void GateOpen() 
    {

        if (_Lightingintensity == 1 && _moonIntensity == 80 && _moonSpotIntensity == 8 && _directionalLightIntensity == 1) 
        {
            FullLightUp.Invoke();
        }
    }
}