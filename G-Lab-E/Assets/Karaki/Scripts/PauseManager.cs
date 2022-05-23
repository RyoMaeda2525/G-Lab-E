using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>IPausableを継承しているコンポーネントを一時停止させるコンポーネント</summary>
public class PauseManager : MonoBehaviour
{
    /// <summary>true : ポーズ中</summary>
    public static bool IsPausing = false;

    [SerializeField, Tooltip("ポーズ実施時に実行するメソッド")]
    UnityEvent OnPausing = default;

    [SerializeField, Tooltip("ポーズ解除時に実行するメソッド")]
    UnityEvent UnPaused = default;

    /// <summary>timeScaleをinspecterからいじれるようにするためのプロパティ</summary>
    public float TimeScale { get => Time.timeScale; set => Time.timeScale = value; }

    
    void Start()
    {
        UnPaused.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputUtility.GetDownPauseMenu)
        {
            if (IsPausing)
            {
                Time.timeScale = 1f;
                IsPausing = false;
                UnPaused.Invoke();
            }
            else
            {
                Time.timeScale = 0f;
                IsPausing = true;
                OnPausing.Invoke();
            }
        }
    }

    /// <summary>ポーズ解除要請メソッド</summary>
    public void BackFromPauseOrder()
    {
        Time.timeScale = 1f;
        IsPausing = false;
        UnPaused.Invoke();
    }
}
