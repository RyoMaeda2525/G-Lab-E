using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestTriggerEvent : MonoBehaviour
{
    Collider _Trigger = default;

    [SerializeField, Tooltip("プレイヤーのタグ名")]
    string _TagNamePlayer = "Player";

    [SerializeField, Tooltip("実行するイベントのメソッドをここにアサイン")]
    UnityEvent _RunEvent = default;

    // Start is called before the first frame update
    void Start()
    {
        _Trigger = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(_TagNamePlayer))
        {
            _RunEvent.Invoke();
        }
    }
}
