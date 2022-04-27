using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TriggerEvent))]
public class CourseOutRecover : MonoBehaviour
{
    [SerializeField, Tooltip("復活地点となる位置をアサイン")]
    Transform _RecoverPoint = default;

    /// <summary>復活対象</summary>
    Transform _RecoverTarget = default;

    [SerializeField, Tooltip("暗転させるメソッドをアサイン")]
    protected UnityEvent _DoBlackoutMethod = default;

    [SerializeField, Tooltip("暗転から復帰するメソッドをアサイン")]
    protected UnityEvent _ReturnFromBlackoutMethod = default;

    /// <summary>起因するイベントトリガーコンポーネント</summary>
    TriggerEvent _Trigger = default;

    // Start is called before the first frame update
    void Start()
    {
        _Trigger = GetComponent<TriggerEvent>();
    }

    public void DoRecover()
    {
        StartCoroutine(RecoverCoroutine());
    }

    /// <summary>コースに復帰する流れを実施するコルーチン</summary>
    protected virtual IEnumerator RecoverCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        _DoBlackoutMethod.Invoke();

        yield return wait;

        _Trigger.Triggered.transform.position = _RecoverPoint.position;

        yield return wait;

        _ReturnFromBlackoutMethod.Invoke();
    }
}
