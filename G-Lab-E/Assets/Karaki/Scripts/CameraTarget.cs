using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;
using Cinemachine.PostFX;


public class CameraTarget : MonoBehaviour
{
    CinemachineFreeLook _Cfl = default;

    GameObject[] players = default;

    GameObject target = default;

    CinemachinePostProcessing _Cpp = default;

    PostProcessProfile _Origin = default;

    [SerializeField]
    PostProcessProfile _ProfileEmitEcho = default;

    /// <summary> エコーロケーション要求フラグ </summary>
    static bool _IsEcho = false;

    /// <summary> エコーロケーションを走らせているコルーチン格納メンバー </summary>
    Coroutine EchoProcess = null;

    public static bool IsEcho { get => _IsEcho; }

    public static void DoEchoOrder()
    {
        _IsEcho = true;
    }

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        _Cfl = GetComponent<CinemachineFreeLook>();
        _Cpp = GetComponent<CinemachinePostProcessing>();
        _Origin = _Cpp.m_Profile;
    }

    // Update is called once per frame
    void Update()
    {
        if (!target || !target.activeSelf)
        {
            FindTarget();
        }

        if (_IsEcho)
        {
            if (EchoProcess == null)
            {
                EchoProcess = StartCoroutine(RunEchoSystem());
            }
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

    IEnumerator RunEchoSystem()
    {
        _Cpp.m_Profile = _ProfileEmitEcho;
        _Cpp.InvalidateCachedProfile();

        yield return new WaitForSeconds(3f);

        _Cpp.m_Profile = _Origin;
        _Cpp.InvalidateCachedProfile();

        EchoProcess = null;
        _IsEcho = false;
    }
}
