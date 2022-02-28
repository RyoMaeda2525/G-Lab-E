using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    [SerializeField] float speed;
    public float jumpPower;
    public Rigidbody rb;

    private GameObject mainCamera;              //メインカメラ格納用
    [SerializeField] private GameObject playerObject;            //回転の中心となるプレイヤー格納用
    public float rotateSpeed = 2.0f;
    // Start is called before the first frame update

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotateCameraの呼び出し
        rotateplayer();

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
    private void rotateplayer()
    {
        //Vector3でX,Y方向の回転の度合いを定義
        Vector3 angle = new Vector3(Input.GetAxis("Mouse X") * rotateSpeed, Input.GetAxis("Mouse Y") * rotateSpeed, 0);

        playerObject.transform.RotateAround(playerObject.transform.position, Vector3.up, angle.x);
        playerObject.transform.RotateAround(playerObject.transform.position, transform.right, angle.y);
    }
}
