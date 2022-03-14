using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcholocationController1 : MonoBehaviour
{
    private static readonly int Center = Shader.PropertyToID("_Center");
    private static readonly int Radius = Shader.PropertyToID("_Radius");

    // ここにインスペクター上でEcholocationマテリアルをセットしておく
    [SerializeField] Material material;

    // 半径が大きくなるスピード
    [SerializeField] [Min(0.0f)] float speed = 5.0f;
    [SerializeField] GameObject _player;
    Vector3 _playerPos;

    // 現在の半径
    private float radius;

    // 半径の初期値は無限大としておく
    private void Start()
    {
        _playerPos = _player.transform.position;
    }
    private void OnEnable()
    {
        this.radius = Mathf.Infinity;
    }

    // 毎フレーム半径のセットおよび拡張を行う
    private void Update()
    {
        _playerPos = _player.transform.position;
        if (InputUtility.GetDownJump)
        {
            Debug.Log("ジャンプボタンが押された！");
            EchoMaterialSwitcher.DoEchoOrder();
            EmitCall(_playerPos);
        }

        this.material.SetFloat(Radius, this.radius);
        this.radius += this.speed * Time.deltaTime;
    }

    // 他のスクリプトからEmitCallを実行することで
    // 中心点を設定し、半径を0にリセットする
    public void EmitCall(Vector3 position)
    {
        this.radius = 0.0f;
        this.material.SetVector(Center, position);
    }
}
