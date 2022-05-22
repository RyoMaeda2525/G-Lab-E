﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleUI : MonoBehaviour
{
    [SerializeField, Tooltip("実際に押すボタン")]
    GameObject[] _Buttons = new GameObject[4];

    [SerializeField, Tooltip("選択状態になったUI")]
    GameObject[] _uiImages = new GameObject[4];

    [SerializeField]
    EventSystem _es = default;

    [SerializeField, Tooltip("最初に選択するボタンの要素数")]
    int _startButtonNumber = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update() //選択されているボタンに合った画像を出す
    {
        for (int i = 0; i < _Buttons.Length; i++) 
        {
            if (_es.currentSelectedGameObject == _Buttons[i])
            {
                _uiImages[i].SetActive(true);
            }
            else
            {
                _uiImages[i].SetActive(false);
            }
        }
    }

    public void StartUI()　//_startButtonNumberで決めたボタンを初めに選択
    {
        _es.SetSelectedGameObject(_Buttons[_startButtonNumber]);
    }


}