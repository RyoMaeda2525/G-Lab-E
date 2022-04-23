using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class movewithfloor : MonoBehaviour
{
    public Vector3 defaultScale = Vector3.zero;
    [SerializeField] PostProcessVolume postProcessVolume;
    [SerializeField] float comHeight;
    [SerializeField] GameObject stage_3F;
    [SerializeField] GameObject stage_3F_echo;

    void Start()
    {
        defaultScale = transform.lossyScale;
        stage_3F_echo.SetActive(false);
    }

    void Update()
    {
        Vector3 lossScale = transform.lossyScale;
        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(
                localScale.x / lossScale.x * defaultScale.x,
                localScale.y / lossScale.y * defaultScale.y,
                localScale.z / lossScale.z * defaultScale.z
        );
    }

    void OnCollisionEnter(Collision collision)
    {

        if (transform.parent == null && collision.gameObject.tag == "MoveFloor")
        {
            var emptyObject = new GameObject();
            emptyObject.transform.parent = collision.gameObject.transform;
            transform.parent = emptyObject.transform;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (transform.parent != null && collision.gameObject.tag == "MoveFloor")
        {
            transform.parent = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (other.gameObject.tag == "PostProcessing")
        {
            rb.useGravity = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PostProcessing")
        {
            if (stage_3F.activeSelf == true)
            {
                float playerHeight = transform.position.y + comHeight;
                ColorGrading colorGrading = ScriptableObject.CreateInstance<ColorGrading>(); ;
                colorGrading.enabled.Override(true);
                colorGrading.postExposure.Override(playerHeight);
                postProcessVolume = PostProcessManager.instance.QuickVolume(10, 0f, colorGrading);
                if (Input.GetKey(KeyCode.Q))
                {
                    RuntimeUtilities.DestroyVolume(postProcessVolume, true, true);
                    stage_3F.SetActive(false);
                    stage_3F_echo.SetActive(true);
                }
            }
            else if (stage_3F.activeSelf == false)
            {
                ColorGrading colorGrading = ScriptableObject.CreateInstance<ColorGrading>(); ;
                colorGrading.enabled.Override(true);
                colorGrading.postExposure.Override(0);
                postProcessVolume = PostProcessManager.instance.QuickVolume(10, 0f, colorGrading);
                if (Input.GetKey(KeyCode.Q))
                {
                    stage_3F.SetActive(true);
                    stage_3F_echo.SetActive(false);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        RuntimeUtilities.DestroyVolume(postProcessVolume, true, true);
    }
}
