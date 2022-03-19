using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGameManager : MonoBehaviour
{
    [SerializeField] string _title, _stage1, _stage2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Title()
    {
        SceneManager.LoadScene(_title);
    }
    public void Stage1()
    {
        SceneManager.LoadScene(_stage1);
    }
    public void Stage2()
    {
        SceneManager.LoadScene(_stage2);
    }
}
