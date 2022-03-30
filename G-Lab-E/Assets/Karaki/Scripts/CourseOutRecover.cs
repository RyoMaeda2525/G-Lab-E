using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CourseOutRecover : MonoBehaviour
{
    [SerializeField, Tooltip("復活地点となる位置をアサイン")]
    Transform _RecoverPoint = default;

    /// <summary>復活対象</summary>
    Transform _RecoverTarget = default;

    [SerializeField, Tooltip("暗転させるメソッドをアサイン")]
    UnityEvent _DoBlackoutMethod = default;

    [SerializeField, Tooltip("暗転から復帰するメソッドをアサイン")]
    UnityEvent _ReturnFromBlackoutMethod = default;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoRecover()
    {
        StartCoroutine(RecoverCoroutine());
    }

    IEnumerator RecoverCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        _DoBlackoutMethod.Invoke();

        yield return wait;

        _RecoverTarget.position = _RecoverPoint.position;

        yield return wait;

        _ReturnFromBlackoutMethod.Invoke();
    }
}
