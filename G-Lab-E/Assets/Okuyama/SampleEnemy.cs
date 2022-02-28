using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleEnemy : MonoBehaviour
{
    NavMeshAgent m_agent;
    Vector3 _playerPos;
    Transform _playerTransform;
    [SerializeField] float speed;
    [SerializeField] GameObject _playerObj;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        //_playerPos = _playerObj.transform.position;
        //_playerTransform = _playerObj.transform;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_playerTransform.position - transform.position), 0.3f);

        ////targetに向かって進む
        ////transform.position += transform.forward * speed;
        //m_agent.SetDestination(transform.position += transform.forward * speed);
        m_agent.destination = _playerObj.transform.position;
    }
}
