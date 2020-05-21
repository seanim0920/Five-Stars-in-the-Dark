using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextSceneOnVideoEnd : MonoBehaviour
{
    // Start is called before the first frame update
    public static UnityEngine.Video.VideoPlayer videoPlayer;
    public static void playVideo()
    {
        videoPlayer.Play();
    }

    private void Awake()
    {
        UnityEngine.Video.VideoPlayer videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.Prepare();
        videoPlayer.loopPointReached += MovieFinished;
        videoPlayer.Play();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void MovieFinished(UnityEngine.Video.VideoPlayer vp)
    {
        LoadScene.LoadNextScene();
    }
}
