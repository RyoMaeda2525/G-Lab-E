using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField, Tooltip("復活する場所")] GameObject RespawnPosition = default;
    [SerializeField,Tooltip("暗くなるまでの時間")] int BlackInterval = 10;
    //[SerializeField] bool respawnBool = false;

    public void ResPawnSet() 
    {
        StartCoroutine("ResPawn");
    }

    IEnumerable ResPawn() 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        yield return new WaitForSeconds(BlackInterval);

        player.transform.position = RespawnPosition.transform.position;

    }
}
