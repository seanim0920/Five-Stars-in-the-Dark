using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceSwitch : MonoBehaviour
{
    public AudioSource voice1;
    public AudioSource voice2;
    private bool voiceOne = true;
    void Start()
    {
        if (Random.Range(0, 2) == 0)
        {
            voice1.volume = 0;
            voice2.volume = 1;
        } else
        {
            voice1.volume = 1;
            voice2.volume = 0;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (voice1.volume > 0)
            {
                voice1.volume = 0;
                voice2.volume = 1;
            } else
            {
                voice1.volume = 1;
                voice2.volume = 0;
            }
        }
    }
}