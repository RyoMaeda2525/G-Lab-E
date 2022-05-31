using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField, Tooltip("音量マスを入れる配列")]
    GameObject[] _volumes = new GameObject[10];

    internal static int  _volumeLevel = 8; //現在の音量

    // Start is called before the first frame update
    void Start()
    {
        ColorChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputUtility.GetDownMorphLeft ) 
        {
            if (_volumeLevel > 0) 
            {
                _volumeLevel--;
                ColorChange();
            }
        }
        else if (InputUtility.GetDownMorphRight)
        {
            if (_volumeLevel < 10) 
            {
                _volumeLevel++;
                ColorChange();
            }        
        }
    }

    private void ColorChange() 
    {
        for (int i = 10; i > 0; i--)
        {
            if (i > _volumeLevel)
            {
                _volumes[i - 1].GetComponent<Image>().color = new Color(51f / 255f, 51f / 255f, 51f / 255f); 
                
            }
            else
            {
                _volumes[i - 1].GetComponent<Image>().color = new Color(1f , 1f , 1f);
            }
        }
        CriAtom.SetCategoryVolume("BGM", 0.1f * _volumeLevel);
        CriAtom.SetCategoryVolume("SE", 0.1f * _volumeLevel);
    }
}
