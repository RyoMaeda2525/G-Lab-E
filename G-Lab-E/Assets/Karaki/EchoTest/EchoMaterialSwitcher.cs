using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class EchoMaterialSwitcher : MonoBehaviour
{
    /// <summary> エコーロケーション要求フラグ </summary>
    static bool _IsEcho = false;

    [SerializeField, Tooltip("制御対象のRenderer")]
    Renderer _Renderer = default;

    [SerializeField, Tooltip("元のマテリアル")]
    Material _MaterialOrigin = default;

    [SerializeField, Tooltip("Echo用マテリアル")]
    Material _MaterialEcho = default;

    /// <summary> エコーロケーションを走らせているコルーチン格納メンバー </summary>
    Coroutine EchoProcess = null;

    public static void DoEchoOrder()
    {
        _IsEcho = true;
    }

    IEnumerator RunEchoSystem()
    {
        _Renderer.material = _MaterialEcho;

        yield return new WaitForSeconds(5f);

        _Renderer.material = _MaterialOrigin;

        EchoProcess = null;
        _IsEcho = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _Renderer = GetComponent<Renderer>();
        _MaterialOrigin = _Renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (_IsEcho)
        {
            if (EchoProcess == null)
            {
                EchoProcess = StartCoroutine(RunEchoSystem());
            }
        }
    }
}
