using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleEnemy : MonoBehaviour
{
    /// <summary>ナビメッシュ</summary>
    NavMeshAgent m_agent;
    /// <summary>エネミーのスピード</summary>
    //[SerializeField] float speed = 0.5f;
    /// <summary>追いかけるプレイヤー</summary>
    [SerializeField] GameObject _playerObj;
    private RaycastHit hit;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
      
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
                if (hit.transform.gameObject == _playerObj)
                {
                    m_agent.isStopped = false;
                    m_agent.destination = _playerObj.transform.position;
                    Debug.Log("当たった");
                }
                else
                {
                    m_agent.isStopped = true;
                }

            }

        }
    }
}
