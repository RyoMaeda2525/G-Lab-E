using System.Linq;
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

    [SerializeField, Tooltip("元のマテリアル(複数)")]
    Material[] _MaterialOrigins = default;

    [SerializeField, Tooltip("Echo用マテリアル")]
    Material _MaterialEcho = default;

    [SerializeField, Tooltip("Echo用マテリアル(複数)")]
    Material[] _MaterialEchos = default;

    /// <summary> エコーロケーションを走らせているコルーチン格納メンバー </summary>
    Coroutine EchoProcess = null;

    public static bool IsEcho { get => _IsEcho; }

    public static void DoEchoOrder()
    {
        _IsEcho = true;
    }

    IEnumerator RunEchoSystem()
    {
        _Renderer.materials = _MaterialEchos;

        yield return new WaitForSeconds(3f);

        _Renderer.materials = _MaterialOrigins;

        EchoProcess = null;
        _IsEcho = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _Renderer = GetComponent<Renderer>();
        _MaterialOrigins = _Renderer.materials;
        _MaterialEchos = new Material[_MaterialOrigins.Length];
        _MaterialEchos = _MaterialEchos.Select(m => m = _MaterialEcho).ToArray();
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
