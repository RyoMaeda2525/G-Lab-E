using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    [SerializeField] float speed;
    public float jumpPower;
    public Rigidbody rb;
    // Start is called before the first frame update

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0f, 0f, 0.1f * speed);
            //Instantiate(originObject, this.transform.position, this.transform.rotation);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0f, 0f, -0.1f * speed);
            //Instantiate(originObject, this.transform.position, this.transform.rotation);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-0.1f * speed, 0f, 0f);
            //Instantiate(originObject, this.transform.position, this.transform.rotation);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(0.1f * speed, 0f, 0f);
            //Instantiate(originObject, this.transform.position, this.transform.rotation);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0f, 0.1f, 0f) * jumpPower, ForceMode.Impulse);
            //Instantiate(originObject, this.transform.position, this.transform.rotation);
        }
    }
}
