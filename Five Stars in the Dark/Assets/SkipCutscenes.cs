using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkipCutscenes : MonoBehaviour
{
    public static bool isSkipping = false;

    public AudioSource levelDialogue;
    public AudioSource skipStartSound;
    public AudioSource skipLoopSound;
    public AudioSource skipEndSound;
    // Start is called before the first frame update
    void Start()
    {
        
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
        if (!CountdownTimer.getTracking())
        {
            isSkipping = true;
            skipStartSound.Play();
            skipLoopSound.Play();
            levelDialogue.pitch = 50;
            while (!CountdownTimer.getTracking())
            {
                yield return new WaitForSeconds(0);
            }
            levelDialogue.pitch = 1;
            skipEndSound.Play();
            skipLoopSound.Stop();
            isSkipping = false;
            print(levelDialogue.time);
        }
    }
}
