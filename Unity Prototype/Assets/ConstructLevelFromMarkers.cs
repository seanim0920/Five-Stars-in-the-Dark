using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructLevelFromMarkers : MonoBehaviour
{
    //for skipping dialogue
    public int dialogueSection = 0;
    Coroutine cutsceneRoutine = null;

    AudioSource level;
    List<float> dialogueTimes = new List<float>();
    public SteeringWheelControl wheelFunctions;

    //for lowering the volume when dialogue is playing
    public Transform leftSpeaker;
    public Transform rightSpeaker;
    public bool isSpeaking;
    private string[] dialogueInstruments = { "Drums", "Support", "Wind" };
    public float maxVol = 0.8f;

    //for the start cutscene
    public PlayerControls controls;
    public AudioSource ambience;
    public GameObject blackScreen;
    public AudioSource carStart;

    // Start is called before the first frame update
    void Start()
    {
        level = GetComponent<AudioSource>();
        TextAsset markers = Resources.Load<TextAsset>("Level1Markers");
        string[] lines = markers.text.Split('\n');
        foreach (string line in lines)
        {
            string[] tokens = line.Split(new char[] { ' ', '\t' });
            int lineLength;
            if (tokens.Length >= 3)
            {
                //print(tokens[0]);
                //start of dialogue
                dialogueTimes.Add(float.Parse(tokens[0]));
                //end of dialogue
                dialogueTimes.Add(float.Parse(tokens[1]));
                //time to wait after playing dialogue
                dialogueTimes.Add((tokens.Length == 4) ? float.Parse(tokens[3]) : 0);
            }
        }

        cutsceneRoutine = StartCoroutine(startCutscenes(dialogueSection));
        StartCoroutine(lockWheel());
        StartCoroutine(shiftLoopSectionOfMusic(16f, 70.5f));
    }

    IEnumerator startCutscenes(int startSection)
    {
        for (int i = startSection*3; i < dialogueTimes.Count; i += 3)
        {            
            //function should go here
            if (startSection == 0)
            {
                blackScreen.SetActive(true);
            }
            else
            {
                ambience.Play();
                carStart.Play();
                controls.enabled = true;
            }

            level.time = dialogueTimes[i];
            level.Play();
            adjustInstrumentVolume(true, new string[] { });
            print("Waiting for " + (dialogueTimes[i + 1] - dialogueTimes[i]) + " seconds");
            yield return new WaitForSeconds(dialogueTimes[i + 1] - dialogueTimes[i]);
            print("Finished waiting");
            level.Pause();
            adjustInstrumentVolume(false, new string[] { });
            yield return new WaitForSeconds(dialogueTimes[i + 2]);

            //function should go here
            if (i / 3 == 0)
            {
                blackScreen.SetActive(false);
            }
            if (i / 3 == 1)
            {
                yield return new WaitForSeconds(1);
                carStart.Play();
                StartCoroutine(wheelRumble());
                yield return new WaitForSeconds(1);
                controls.enabled = true;
            }

            //if (i/3 == 6) {
            //    changeInstrumentVolume(0, "all");
            //    leftnews.Play();
            //    rightnews.Play();
            //    yield return new WaitForSeconds(rightnews.clip.length - 2);
            //    changeInstrumentVolume(1, "all");
            //    volumeAdjust(false);
            //    part6.Play();
            //}
        }
    }

    IEnumerator lockWheel()
    {
        while (!controls.enabled)
        {
            wheelFunctions.StopSoftstopForce();
            yield return new WaitForSeconds(0);
        }
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

    void adjustInstrumentVolume(bool dialogueStart, string[] instruments)
    {
        isSpeaking = dialogueStart;
        for (int loop = 0; loop < 2; loop++)
        {
            Transform speaker = leftSpeaker;
            if (loop > 0) speaker = rightSpeaker;
            foreach (Transform child in speaker)
            {
                if (instruments.Length == 0 || System.Array.IndexOf(instruments, child.gameObject.name) > -1)
                {
                    if (dialogueStart)
                    {
                        print("pause music");
                        child.gameObject.GetComponent<AudioSource>().volume = 0.5f;
                    }
                    else
                    {
                        print("resume music");
                        child.gameObject.GetComponent<AudioSource>().volume = 1f;
                    }
                }
            }
        }
    }
    IEnumerator shiftLoopSectionOfMusic(float startTime, float endTime)
    {
        while (true)
        {
            for (int loop = 0; loop < 2; loop++)
            {
                Transform speaker = leftSpeaker;
                if (loop > 0) speaker = rightSpeaker;
                foreach (Transform child in speaker)
                {
                    AudioSource instrument = child.gameObject.GetComponent<AudioSource>();
                    if (instrument.time >= endTime)
                    {
                        child.gameObject.GetComponent<AudioSource>().time = startTime;
                    }
                }
            }
            yield return null;
        }
    }
    public bool checkIfSpeaking()
    {
        return isSpeaking;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            StopCoroutine(cutsceneRoutine);
            cutsceneRoutine = StartCoroutine(startCutscenes(dialogueSection++));
        }
    }
}