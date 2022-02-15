using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0f, 0f, 0.1f * speed);
            //Instantiate(originObject, this.transform.position, this.transform.rotation);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0f, 0f, -0.1f * speed);
            //Instantiate(originObject, this.transform.position, this.transform.rotation);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-0.1f * speed, 0f, 0f);
            //Instantiate(originObject, this.transform.position, this.transform.rotation);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(0.1f * speed, 0f, 0f);
            //Instantiate(originObject, this.transform.position, this.transform.rotation);
        }
    }
}
