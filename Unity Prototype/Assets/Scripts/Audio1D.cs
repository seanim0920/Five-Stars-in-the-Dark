using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio1D : MonoBehaviour
{
    private AudioMixer mixer;
    //public AudioMixerGroup mixerGroup;
    private AudioSource soundObj;
    private Camera viewport;
    private float hearingDistance = 6;
    // Start is called before the first frame update

    void Start()
    {
        viewport = Camera.main;
        soundObj = GetComponent<AudioSource>();
        mixer = Instantiate(soundObj.outputAudioMixerGroup.audioMixer);
        //soundObj.outputAudioMixerGroup.audioMixer = mixer;
    }

    // Update is called once per frame
    void Update()
    {
        mixer.SetFloat("PitchShift", ((1/(1+Mathf.Exp(3*(-transform.position.x+6))))+1) / soundObj.pitch);
    }
}