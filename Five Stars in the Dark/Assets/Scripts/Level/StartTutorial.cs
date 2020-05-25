using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StartTutorial : MonoBehaviour
{
    private AudioSource levelAudio;
    // Start is called before the first frame update
    void Start()
    {
        levelAudio = gameobject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(levelAudio.time >= levelAudio.clip.length)
        {
            Instantiate()
        }
    }
}
