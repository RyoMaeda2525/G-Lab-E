using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EcholocationController1 : MonoBehaviour
{
    /// <summary>マテリアルのパラメーター、_Center のID</summary>
    static readonly int IDCenter = Shader.PropertyToID("_Center");

    /// <summary>マテリアルのパラメーター、_Radius のID</summary>
    static readonly int IDRadius = Shader.PropertyToID("_Radius");

    [SerializeField, Tooltip("ここにインスペクター上でEcholocationマテリアルをセットしておく")]
    Material _Material = default;

    [SerializeField, Min(0.0f), Tooltip("半径が大きくなるスピード")] 
    float _Speed = 5.0f;

    [SerializeField, Tooltip("Echolocationするキャラクター")]
    GameObject _Player;

    /// <summary>現在の半径</summary>
    float _Radius;

    /// <summary>True : エコーロケーションを行った</summary>
    bool _DoneEcho = false;

    [SerializeField, Tooltip("エコー開始時に実行するメソッドを指定")]
    UnityEvent _MethodForStartEcho = default;

    [SerializeField, Tooltip("エコー終了時に実行するメソッドを指定")]
    UnityEvent _MethodForEndEcho = default;

    /// <summary>True : エコー終了時に実行するメソッドを実行した</summary>
    bool _DoneEndEchoMethod = true;

    /// <summary>True : エコーロケーションを行った。取得後Falseになる</summary>
    public bool DoneEcho { get => _DoneEcho; }

    
    private void Start()
    {
        // 半径の初期値は無限大としておく
        _Radius = Mathf.Infinity;
    }

    private void OnEnable()
    {
        // 半径の初期値は無限大としておく
        _Radius = Mathf.Infinity;
    }

    // 毎フレーム半径のセットおよび拡張を行う
    private void Update()
    {
        _DoneEcho = false;
        Vector3 _playerPos = _Player.transform.position;
        if (!_DoneEcho && !CameraTarget.IsEcho && InputUtility.GetDownJump)
        {
            CameraTarget.DoEchoOrder();
            EmitCall(_playerPos);
            _MethodForStartEcho.Invoke();
            _DoneEndEchoMethod = false;
            _DoneEcho = true;
        }
        else if(!_DoneEndEchoMethod && !CameraTarget.IsEcho)
        {
            _MethodForEndEcho.Invoke();
            _DoneEndEchoMethod = true;
        }

        _Material.SetFloat(IDRadius, _Radius);
        _Radius += _Speed * Time.deltaTime;
    }

    /// <summary>
    /// 他のスクリプトからEmitCallを実行することで中心点を設定し、半径を0にリセットする
    /// </summary>
    /// <param name="position">Echoの発生源</param>
    public void EmitCall(Vector3 position)
    {
        _Radius = 0.0f;
        _Material.SetVector(IDCenter, position);
    }
}
