using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    VideoPlayer vp = default;

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        vp.Pause();
    }

    // Update is called once per frame
    public void VideoStart()
    {
        vp.Play();
    }

    public void VideoStop() 
    {
        vp.Stop();
    }
}
