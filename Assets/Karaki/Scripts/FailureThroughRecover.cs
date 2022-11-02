using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FailureThroughRecover : CourseOutRecover
{
    [SerializeField, Tooltip("CinemachineDollyCartで、通過失敗した際に復帰する位置をセットするメソッドをアサイン")]
    UnityEvent _SetPosition = default;

    [SerializeField, Tooltip("通過失敗復帰処理で、見えなくしたオブジェクトを見えるようにするための処理をアサイン")]
    UnityEvent _ObjectActivate = default;

    /// <summary>コースに復帰する流れを実施するコルーチン</summary>
    protected override IEnumerator RecoverCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        _DoBlackoutMethod.Invoke();

        yield return wait;

        _SetPosition.Invoke();

        yield return wait;

        _ObjectActivate.Invoke();
        _ReturnFromBlackoutMethod.Invoke();
    }
}
