using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Cutscenes : MonoBehaviour
{
    public bool isSpeaking = false;

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
    public AudioSource part6;
    public AudioSource leftnews;
    public AudioSource rightnews;
    public AudioSource gps_start;
    public AudioSource gps_near;
    public AudioSource gps_end;
    public float maxVol = 0.8f;
    private string[] dialogueInstruments = { "Drums", "Support", "Wind" };
    // Start is called before the first frame update
    private int engineStart = 0;
    void Start()
    {
        controls = GetComponent<PlayerControls>();
        StartCoroutine(startLevel());
        //each phase of dialogue plays a different section of music
        foreach (Transform child in leftSpeaker)
        {
            child.gameObject.GetComponent<AudioSource>().time = 16;
        }
        foreach (Transform child in rightSpeaker)
        {
            child.gameObject.GetComponent<AudioSource>().time = 16;
        }
    }

    IEnumerator startLevel()
    {
        if (state == 0)
        {
            changeInstrumentVolume(0, "all");
            intro.Play();
            yield return new WaitForSeconds(34);
            blackScreen.SetActive(false);
            yield return new WaitForSeconds(intro.clip.length - 35);
            state = 1;
        }
        blackScreen.SetActive(false);
        ambience.Play();
        controls.enabled = true;
        changeInstrumentVolume(1, "all");
        startCar.Play();
        StartCoroutine(wheelRumble());

        yield return new WaitForSeconds(5);

        volumeAdjust(true);
        part1.time = 5;
        part1.Play();
        yield return new WaitForSeconds(part1.clip.length);
        gps_start.Play();
        yield return new WaitForSeconds(gps_start.clip.length);
        volumeAdjust(false);
        yield return new WaitForSeconds(7);
        volumeAdjust(true);
        part2.Play();
        yield return new WaitForSeconds(part2.clip.length);
        volumeAdjust(false);
        yield return new WaitForSeconds(20);
        volumeAdjust(true);
        part3.Play();
        yield return new WaitForSeconds(part3.clip.length);
        volumeAdjust(false);
        yield return new WaitForSeconds(10);
        volumeAdjust(true);
        part4.Play();
        yield return new WaitForSeconds(part4.clip.length);
        volumeAdjust(false);
        yield return new WaitForSeconds(17);
        volumeAdjust(true);
        gps_near.Play();
        yield return new WaitForSeconds(gps_near.clip.length);
        part5.Play();
        changeInstrumentVolume(0, "all");
        leftnews.Play();
        rightnews.Play();
        yield return new WaitForSeconds(rightnews.clip.length-2);
        changeInstrumentVolume(1, "all");
        volumeAdjust(false);
        part6.Play();
        yield return new WaitForSeconds(part6.clip.length+4);
        gps_end.Play();
        SceneManager.LoadScene("EndScreen", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
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

    void volumeAdjust(bool dialogueStart)
    {
        isSpeaking = dialogueStart;
        if (dialogueStart)
        {
            foreach (Transform child in leftSpeaker)
            {
                if (System.Array.IndexOf(dialogueInstruments, child.gameObject.name) > -1)
                    child.gameObject.GetComponent<AudioSource>().volume = 0.5f;
            }
            foreach (Transform child in rightSpeaker)
            {
                if (System.Array.IndexOf(dialogueInstruments, child.gameObject.name) > -1)
                    child.gameObject.GetComponent<AudioSource>().volume = 0.5f;
            }
        } else
        {
            foreach (Transform child in leftSpeaker)
            {
                if (System.Array.IndexOf(dialogueInstruments, child.gameObject.name) > -1)
                    child.gameObject.GetComponent<AudioSource>().volume = 1;
            }
            foreach (Transform child in rightSpeaker)
            {
                if (System.Array.IndexOf(dialogueInstruments, child.gameObject.name) > -1)
                    child.gameObject.GetComponent<AudioSource>().volume = 1;
            }
        }
    }

    public void changeInstrumentVolume(float vol, string name)
    {
        foreach (Transform child in leftSpeaker)
        {
            if (child.name == name || name == "all")
                child.gameObject.GetComponent<AudioSource>().volume = vol;
        }
        foreach (Transform child in rightSpeaker)
        {
            if (child.name == name || name == "all")
                child.gameObject.GetComponent<AudioSource>().volume = vol;
        }
    }

    void loopInstruments(int state)
    {
        foreach (Transform child in leftSpeaker)
        {
            AudioSource instrument = child.gameObject.GetComponent<AudioSource>();
            if (instrument.time >= 70.5)
            {
                child.gameObject.GetComponent<AudioSource>().time = 16;
            }
        }
        foreach (Transform child in rightSpeaker)
        {
            AudioSource instrument = child.gameObject.GetComponent<AudioSource>();
            if (instrument.time >= 70.5)
            {
                child.gameObject.GetComponent<AudioSource>().time = 16;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        loopInstruments(0);
        if (state == 0) wheelFunctions.PlaySoftstopForce(1);
    }
    public bool checkIfSpeaking()
    {
        return isSpeaking;
    }
}