using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraTarget : MonoBehaviour
{
    CinemachineFreeLook _Cfl = default;

    GameObject[] players = default;

    GameObject target = default;

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        _Cfl = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!target || !target.activeSelf)
        {
            FindTarget();
        }
    }

    /// <summary> 新しいカメラの注視対象を入手 </summary>
    void FindTarget()
    {
        target = players.Where(p => p.activeSelf).FirstOrDefault();

        if(target)
        {
            _Cfl.Follow = target.transform;
            _Cfl.LookAt = target.transform;
        }
    }
}
