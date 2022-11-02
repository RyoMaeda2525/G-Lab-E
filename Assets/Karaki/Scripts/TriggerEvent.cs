using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    [SerializeField, Tooltip("イベントを発生させる対象オブジェクトのタグ名")]
    string _TagNameTriggerTarget = "Player";

    /// <summary>トリガーに接触したオブジェクト</summary>
    GameObject _Triggered = null;

    [SerializeField, Tooltip("トリガーに触れた直後に実行するイベントのメソッドをここにアサイン")]
    UnityEvent _EnterToRunEvent = default;

    [SerializeField, Tooltip("トリガーに触れている間に実行し続けるイベントのメソッドをここにアサイン")]
    UnityEvent _StayToRunEvent = default;

    [SerializeField, Tooltip("トリガーに触れ、離れた直後に実行するイベントのメソッドをここにアサイン")]
    UnityEvent _ExitToRunEvent = default;

    public GameObject Triggered { get => _Triggered; }

    // Start is called before the first frame update
    void Start()
    {
        _Triggered = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_TagNameTriggerTarget))
        {
            _Triggered = other.gameObject;
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
            _Triggered = null;
            _ExitToRunEvent.Invoke();
        }
    }
}
