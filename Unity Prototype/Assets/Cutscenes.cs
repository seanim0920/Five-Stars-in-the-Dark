using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Cutscenes : MonoBehaviour
{
    public bool isSpeaking = false;

    public SteeringWheelControl wheelFunctions;
    public GameObject blackScreen;
    public int state;
    public AudioSource intro;
    public AudioSource startCar;

    public AudioSource leftnews;
    public AudioSource rightnews;
    // Start is called before the first frame update
    void Start()
    {
        //controls = GetComponent<PlayerControls>();
        //StartCoroutine(startLevel());
        //each phase of dialogue plays a different section of music
    }


    // Update is called once per frame
    void Update()
    {
        //if (state == 0) wheelFunctions.PlaySoftstopForce(1);
    }
    public bool checkIfSpeaking()
    {
        return isSpeaking;
    }
}