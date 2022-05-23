using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CloseInformationWindow : MonoBehaviour
{
    [SerializeField] UnityEvent _RunMethod = default;

    // Update is called once per frame
    void Update()
    {
        if (InputUtility.GetJump)
        {
            _RunMethod.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
