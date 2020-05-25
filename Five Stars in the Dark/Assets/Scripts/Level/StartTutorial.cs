using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StartTutorial : MonoBehaviour
{
    private AudioSource levelAudio;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        levelAudio = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        Debug.Log(player);
    }

    // Update is called once per frame
    void Update()
    {
        if(levelAudio.time == levelAudio.clip.length)
        {
            player.GetComponent<PlayerControls>().enabled = true;
            StartCoroutine(beginLevel());
        }
    }

    void enableControllers()
    {
        if (SettingsManager.toggles[0])
        {
            player.GetComponent<GamepadControl>().enabled = false;
            player.GetComponent<KeyboardControl>().enabled = false;
            player.GetComponent<SteeringWheelInput>().enabled = true;
        }
        else if (SettingsManager.toggles[2])
        {
            player.GetComponent<SteeringWheelInput>().enabled = false;
            player.GetComponent<KeyboardControl>().enabled = false;
            player.GetComponent<GamepadControl>().enabled = true;
        }
        else
        {
            player.GetComponent<SteeringWheelInput>().enabled = false;
            player.GetComponent<GamepadControl>().enabled = false;
            player.GetComponent<KeyboardControl>().enabled = true;
        }
    }

    IEnumerator beginLevel()
    {
        enableControllers();
        yield break;
    }
}
