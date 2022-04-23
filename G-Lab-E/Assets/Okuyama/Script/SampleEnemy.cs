using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleEnemy : MonoBehaviour
{
    /// <summary>ナビメッシュ</summary>
    NavMeshAgent m_agent;
    /// <summary>追いかけるプレイヤー</summary>
    [SerializeField] GameObject _playerObj;
    /// <summary>徘徊してほしい場所</summary>
    [SerializeField] Transform[] _wanderingPoint;
    [SerializeField] int destPoint = 0;

    private RaycastHit hit;
    /// <summary>徘徊 /デフォ 徘徊する</summary>
    bool _wanderingBool = true;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.autoBraking = false;

        GotoNextPoint();
    }

    void Update()
    {
        if (_wanderingBool == true)
        {
            if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }
        }
    }

    void GotoNextPoint()
    {

        Debug.Log(_wanderingBool);
        // 地点がなにも設定されていないときに返します
        if (_wanderingPoint.Length == 0)
            return;

        // エージェントが現在設定された目標地点に行くように設定します
        m_agent.destination = _wanderingPoint[destPoint].position;

        // 配列内の次の位置を目標地点に設定し、
        // 必要ならば出発地点にもどります
        destPoint = (destPoint + 1) % _wanderingPoint.Length;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == _playerObj)
        {
            var diff = _playerObj.transform.position - transform.position;
            var distance = diff.magnitude;
            var direction = diff.normalized;

            if (Physics.Raycast(transform.position, direction, out hit, distance))
            {
                Debug.Log("Rayが当たった");
                _wanderingBool = false;
                m_agent.isStopped = false;
                m_agent.destination = _playerObj.transform.position;
            }
            else
            {
                m_agent.isStopped = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        _wanderingBool = true;;
    }
}
