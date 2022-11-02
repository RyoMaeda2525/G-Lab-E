using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSkinedMeshCollider : MonoBehaviour
{
    private MeshCollider meshCollider;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    public void CreateMeshCollider() //Meshコライダーを生成
    {
        Mesh bakedMesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(bakedMesh);
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = bakedMesh;
    }
}