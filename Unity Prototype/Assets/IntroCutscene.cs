using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
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
    void Start()
    {
        controls = GetComponent<PlayerControls>();
        StartCoroutine(startLevel());
    }

    IEnumerator startLevel()
    {
        //intro.Play();
        //yield return new WaitForSeconds(intro.clip.length);
        ambience.Play();
        controls.enabled = true;
        startCar.Play();
        yield return new WaitForSeconds(1);
        foreach (Transform child in rightSpeaker)
        {
            child.gameObject.GetComponent<AudioSource>().volume = maxVol;
        }
        foreach (Transform child in leftSpeaker)
        {
            child.gameObject.GetComponent<AudioSource>().volume = maxVol;
        }
        yield return new WaitForSeconds(1);
        part1.time = 5;
        part1.Play();
        yield return new WaitForSeconds(part1.clip.length);
        yield return new WaitForSeconds(10);
        part2.Play();
        yield return new WaitForSeconds(part2.clip.length);
        yield return new WaitForSeconds(10);
        part3.Play();
        yield return new WaitForSeconds(part3.clip.length);
        yield return new WaitForSeconds(10);
        part4.Play();
        yield return new WaitForSeconds(part4.clip.length);
        yield return new WaitForSeconds(10);
        part5.Play();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}