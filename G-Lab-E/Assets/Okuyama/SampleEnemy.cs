using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleEnemy : MonoBehaviour
{
    /// <summary>ナビメッシュ</summary>
    NavMeshAgent m_agent;
    Vector3 _playerPos;
    Transform _playerTransform;
    /// <summary>プレイヤーとの距離</summary>
    float distance;
    /// <summary>カメラに映った時のbool</summary>
    bool Rendered = false;
    /// <summary>エネミーが追いかけてくる距離</summary>
    [SerializeField] float _playerdistance;
    /// <summary>エネミーのスピード</summary>
    [SerializeField] float speed;
    /// <summary>追いかけるプレイヤー</summary>
    [SerializeField] GameObject _playerObj;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _playerPos = _playerObj.transform.position;
        distance = Vector3.Distance(this.transform.position, _playerPos);
        
        if(Rendered == true/*distance < _playerdistance*/)
        {
            //m_agent.SetDestination(transform.position += transform.forward * speed);
            m_agent.destination = _playerObj.transform.position;
        }
    }

    /// <summary>メインカメラに映った時に呼ばれる</summary>
    void OnWillRenderObject()
    {
        if (Camera.current.tag == "MainCamera")
        {
            Rendered = true;
        }
    }
}
