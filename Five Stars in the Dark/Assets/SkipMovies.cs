using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkipMovies : MonoBehaviour
{
    public static bool isSkipping = false;

    public AudioSource skipStartSound;
    public AudioSource skipLoopSound;
    public AudioSource skipEndSound;
    private UnityEngine.Video.VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        isSkipping = false;
        if (GetComponent<UnityEngine.Video.VideoPlayer>())
        {
            videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l") || (Gamepad.current != null && Gamepad.current.buttonNorth.isPressed))
        {
            StartCoroutine(skipIntro());
        }
    }

    private IEnumerator skipIntro()
    {
        if (!PauseMenu.isPaused)
        {
            OverlayStatic.overlaid = true;
            isSkipping = true;
            skipStartSound.Play();
            skipLoopSound.Play();
            videoPlayer.playbackSpeed = 10;
            while (videoPlayer.isPlaying && isSkipping)
            {
                yield return new WaitForSeconds(0);
            }
            videoPlayer.playbackSpeed = 1;
            skipEndSound.Play();
            skipLoopSound.Stop();
            isSkipping = false;
            OverlayStatic.overlaid = false;
        }
    }
}
