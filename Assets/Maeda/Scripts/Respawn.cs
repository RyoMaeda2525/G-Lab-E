using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField, Tooltip("復活する場所")] GameObject RespawnPosition = default;
    [SerializeField,Tooltip("暗くなるまでの時間")] float BlackInterval = 10;
    [SerializeField ,Tooltip("タイマー計測開始用")] bool respawnBool = false;

    private float time = 0;

    public void ResPawnTimeSet() 
    {
        respawnBool = true;
    }

    public void ResPawnCall()
    {
        ResPawn();
    }

    private void Update()
    {
        if (respawnBool) 
        {
            time += Time.deltaTime;
            Debug.Log(time);

            if (time > BlackInterval) 
            {
                ResPawn();
            }
        }
    }

    private void ResPawn() //RespawnPositionの場所に移動
    {
        respawnBool = false;
        time = 0;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = RespawnPosition.transform.position;

    }
}
