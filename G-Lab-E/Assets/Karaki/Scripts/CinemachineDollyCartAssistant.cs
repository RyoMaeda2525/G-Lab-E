using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineDollyCart))]
public class CinemachineDollyCartAssistant : MonoBehaviour
{
    CinemachineDollyCart _Cd = default;

    // Start is called before the first frame update
    void Start()
    {
        _Cd = GetComponent<CinemachineDollyCart>();
    }

    public void RecoverPosition(float position)
    {
        _Cd.m_Position = position;
        Debug.Log(position + "\n" + _Cd.m_Position);
    }
}
