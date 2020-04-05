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

    GameObject curb;
    GameObject road;

    // Start is called before the first frame update
    void Start()
    {
        curb = Resources.Load<GameObject>("Curb");
        road = Resources.Load<GameObject>("Road");
        level = GetComponent<AudioSource>();

        TextAsset markers = Resources.Load<TextAsset>("Level1Markers");
        string[] lines = markers.text.Split('\n');

        float totalDialogueTime = 0;
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
                totalDialogueTime += float.Parse(tokens[1]) - float.Parse(tokens[0]) + ((tokens.Length == 4) ? float.Parse(tokens[3]) : 0);
            }
        }

        /*
        TextAsset markers = Resources.Load<TextAsset>("Level1Events");
        string[] lines = markers.text.Split('\n');

        float totalDialogueTime = 0;
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
                totalDialogueTime += float.Parse(tokens[1]) - float.Parse(tokens[0]) + ((tokens.Length == 4) ? float.Parse(tokens[3]) : 0);
            }
        }
        */

        constructLevelMap(totalDialogueTime);

        //function should go here
        if (dialogueSection == 0)
        {
            blackScreen.SetActive(true);
        }
        cutsceneRoutine = StartCoroutine(playCutscenes(dialogueSection));
        StartCoroutine(lockWheel());
        StartCoroutine(shiftLoopSectionOfMusic(16f, 70.5f));
    }

    void constructLevelMap(float totalDialogueTime)
    {
        float width = 10;
        float length = totalDialogueTime / controls.neutralSpeed;
        GameObject roadtile = Instantiate(road, new Vector3(0, 0, 1), Quaternion.identity);
        roadtile.transform.localScale = new Vector3(width, length, 1);
        GameObject leftcurb = Instantiate(curb, new Vector3(-width/3, 0, 1), Quaternion.identity);
        leftcurb.transform.localScale = new Vector3(1,length,1);
        GameObject rightcurb = Instantiate(curb, new Vector3(width/3, 0, 1), Quaternion.identity);
        rightcurb.transform.localScale = new Vector3(1, length, 1);
    }

    IEnumerator playCutscenes(int startSection)
    {
        for (int i = startSection*3; i < dialogueTimes.Count; i += 3)
        {
            if (i == 0)
            {
                StartCoroutine(startCar());
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
            //perform(functions[i]);
        }
    }

    IEnumerator startCar()
    {
        blackScreen.SetActive(false);
        ambience.Play();
        yield return new WaitForSeconds(1);
        carStart.Play();
        StartCoroutine(wheelRumble());
        yield return new WaitForSeconds(1);
        controls.enabled = true;
    }

    IEnumerator lockWheel()
    {
        while (!controls.enabled)
        {
            wheelFunctions.PlaySoftstopForce(100);
            yield return new WaitForSeconds(0);
        }
        wheelFunctions.StopSoftstopForce();
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
            cutsceneRoutine = StartCoroutine(playCutscenes(dialogueSection++));
        }
    }
}