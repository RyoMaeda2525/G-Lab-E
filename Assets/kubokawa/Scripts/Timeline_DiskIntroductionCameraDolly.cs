using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Cinemachine;

public class Timeline_DiskIntroductionCameraDolly : MonoBehaviour, ITimeControl
{
    [SerializeField]
    CinemachineVirtualCamera cvc;
    CinemachineTrackedDolly dolly;

    void Awake()
    {
        dolly = cvc.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    public void OnControlTimeStart()
    {
    }

    public void SetTime(double time)
    {
        dolly.m_PathPosition = (float)time / 1f;
    }

    public void OnControlTimeStop()
    {

    }
}