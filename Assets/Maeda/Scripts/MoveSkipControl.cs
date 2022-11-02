using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MoveSkipControl : MonoBehaviour
{

    [SerializeField, Tooltip("ムービースキップのボタン")]
    GameObject _movieSkipButton = default;

    [SerializeField, Tooltip("ムービースキップのボタン")]
    int _buttonHideTime = 5;

    float timer = 0;

    bool _buttonActiv = false;

    [SerializeField] UnityEvent _RunMethod = default;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (InputUtility.GetDownJump)
        {
            if (_buttonActiv) 
            {
                _RunMethod.Invoke();
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

        if (_buttonActiv)
        {
            timer += Time.deltaTime;
            if (timer > _buttonHideTime)
            {
                _movieSkipButton.SetActive(false);
                _buttonActiv = false;
            }
        }
       
    }

    public void ButtonActiv() 
    {
        _movieSkipButton.SetActive(true);
        _buttonActiv = true;
    } 

}
