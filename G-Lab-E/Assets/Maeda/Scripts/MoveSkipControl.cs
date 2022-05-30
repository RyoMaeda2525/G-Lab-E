using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSkipControl : MonoBehaviour
{

    [SerializeField, Tooltip("ムービースキップのボタン")]
    GameObject _movieSkipButton = default;

    [SerializeField, Tooltip("ムービースキップのボタン")]
    int _buttonHideTime = 5;

    float timer = 0;

    bool _buttonActiv = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_buttonActiv)
        {
            timer += Time.deltaTime;
            if (timer > _buttonHideTime)
            {
                _movieSkipButton.SetActive(false);
                _buttonActiv = false;
            }
        }
        else 
        {
            timer = 0;
            if (InputUtility.GetDownJump) 
            {
                ButtonActiv();
            }
        }
    }

    public void ButtonActiv() 
    {
        _movieSkipButton.SetActive(true);
        _buttonActiv = true;
    } 

}
