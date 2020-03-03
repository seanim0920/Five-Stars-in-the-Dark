using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscenes : MonoBehaviour
{
    public SteeringWheelControl wheelFunctions;
    public GameObject blackScreen;
    public int state;
    public AudioSource intro;
    public AudioSource ambience;
    private PlayerControls controls;
    public AudioSource startCar;
    public Transform leftSpeaker;
    public Transform rightSpeaker;
    public AudioSource part1;
    public AudioSource part2;
    public AudioSource part3;
    public AudioSource part4;
    public AudioSource part5;
    public float maxVol = 0.8f;
    // Start is called before the first frame update
    private int engineStart = 0;
    void Start()
    {
        controls = GetComponent<PlayerControls>();
        StartCoroutine(startLevel());
        //each phase of dialogue plays a different section of music
        loopInstruments(0);
    }

    IEnumerator startLevel()
    {
        if (state == 0) {
            intro.Play();
            yield return new WaitForSeconds(34);
            blackScreen.SetActive(false);
            yield return new WaitForSeconds(intro.clip.length - 35);
            state = 1;
        }
        blackScreen.SetActive(false);
        ambience.Play();
        controls.enabled = true;
        startCar.Play();
        StartCoroutine(wheelRumble());

        yield return new WaitForSeconds(1);
        //changeInstrumentVolume(maxVol, "all");
        yield return new WaitForSeconds(5);
        part1.time = 5;
        part1.Play();
        yield return new WaitForSeconds(part1.clip.length);
        yield return new WaitForSeconds(10);
        part2.Play();
        yield return new WaitForSeconds(part2.clip.length);
        yield return new WaitForSeconds(20);
        part3.Play();
        yield return new WaitForSeconds(part3.clip.length);
        yield return new WaitForSeconds(10);
        part4.Play();
        yield return new WaitForSeconds(part4.clip.length);
        yield return new WaitForSeconds(10);
        part5.Play();
    }

    IEnumerator wheelRumble()
    {
        for (int loop = 0; loop < 25; loop++)
        {
            wheelFunctions.PlayDirtRoadForce(loop * 2);
            yield return new WaitForSeconds(0);
        }
        for (int loop = 50; loop > 10; loop--)
        {
            wheelFunctions.PlayDirtRoadForce(loop);
            yield return new WaitForSeconds(0);
        }
        wheelFunctions.StopDirtRoadForce();
    }

    public void changeInstrumentVolume(float vol, string name)
    {
        foreach (Transform child in leftSpeaker)
        {
            if (child.name == name || name == "all")
                child.gameObject.GetComponent<AudioSource>().volume = maxVol;
        }
        foreach (Transform child in rightSpeaker)
        {
            if (child.name == name || name == "all")
                child.gameObject.GetComponent<AudioSource>().volume = maxVol;
        }
    }

    void loopInstruments(int state)
    {
        foreach (Transform child in leftSpeaker)
        {
            AudioSource instrument = child.gameObject.GetComponent<AudioSource>();
            if (instrument.time >= 70.5) {
                instrument.time = 16;
            }
        }
        foreach (Transform child in rightSpeaker)
        {
            AudioSource instrument = child.gameObject.GetComponent<AudioSource>();
            if (instrument.time >= 70.5)
            {
                instrument.time = 16;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0) wheelFunctions.PlaySoftstopForce(1);
    }
}