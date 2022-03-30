using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    Collider _Trigger = default;

    [SerializeField, Tooltip("イベントを発生させる対象オブジェクトのタグ名")]
    string _TagNameTriggerTarget = "Player";

    [SerializeField, Tooltip("トリガーに触れた直後に実行するイベントのメソッドをここにアサイン")]
    UnityEvent _EnterToRunEvent = default;

    [SerializeField, Tooltip("トリガーに触れている間に実行し続けるイベントのメソッドをここにアサイン")]
    UnityEvent _StayToRunEvent = default;

    [SerializeField, Tooltip("トリガーに触れ、離れた直後に実行するイベントのメソッドをここにアサイン")]
    UnityEvent _ExitToRunEvent = default;

    // Start is called before the first frame update
    void Start()
    {
        _Trigger = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_TagNameTriggerTarget))
        {
            _EnterToRunEvent.Invoke();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(_TagNameTriggerTarget))
        {
            _StayToRunEvent.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_TagNameTriggerTarget))
        {
            _ExitToRunEvent.Invoke();
        }
    }
}
