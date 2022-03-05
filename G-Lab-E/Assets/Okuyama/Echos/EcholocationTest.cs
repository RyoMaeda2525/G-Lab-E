using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EcholocationController))]
public class EcholocationTest : MonoBehaviour
{
    private EcholocationController controller;
    private Camera mainCamera;

    //private void Start()
    //{
    //    this.mainCamera = Camera.main;
    //    this.controller = this.GetComponent<EcholocationController>();
    //}

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0) && Physics.Raycast(this.mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo))
    //    {
    //        this.controller.EmitCall(hitInfo.point);
    //    }
    //}
}
