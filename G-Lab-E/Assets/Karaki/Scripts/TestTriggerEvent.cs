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
    UnityEvent _EnterRunEvent = default;

    [SerializeField, Tooltip("実行するイベントのメソッドをここにアサイン")]
    UnityEvent _SteyRunEvent = default;

    [SerializeField, Tooltip("実行するイベントのメソッドをここにアサイン")]
    UnityEvent _ExitRunEvent = default;

    // Start is called before the first frame update
    void Start()
    {
        _Trigger = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(_TagNamePlayer))
        {
            _EnterRunEvent.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(_TagNamePlayer))
        {
            _SteyRunEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(_TagNamePlayer))
        {
            _ExitRunEvent.Invoke();
        }
    }
}
