using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>IPausableを継承しているコンポーネントを一時停止させるコンポーネント</summary>
public class PauseManager : MonoBehaviour
{
    /// <summary>true : ポーズ中</summary>
    public static bool IsPausing = false;

    // Update is called once per frame
    void Update()
    {
        if (InputUtility.GetDownPauseMenu)
        {
            if (IsPausing)
            {
                Time.timeScale = 1;
                IsPausing = false;
            }
            else
            {
                Time.timeScale = 0;
                IsPausing = true;
            }
        }
    }
}
