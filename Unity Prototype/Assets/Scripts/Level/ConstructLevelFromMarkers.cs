using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class ConstructLevelFromMarkers : MonoBehaviour
{
    public AudioSource levelDialogue;
    public AudioSource secondSource;
    List<string> timedObstacleMarkers = new List<string>();
    List<string> commandMarkers = new List<string>();
    List<string> distanceObstacleMarkers = new List<string>();
    List<string> dialogueMarkers = new List<string>();
    Dictionary<GameObject, float> spawnedObstacles = new Dictionary<GameObject, float>();
    public SteeringWheelControl wheelFunctions;

    //for lowering the volume when dialogue is playing
    public Transform leftSpeaker;
    public Transform rightSpeaker;
    public Transform playerTransform;
    public bool isSpeaking;
    private string[] dialogueInstruments = { "Drums", "Support", "Wind" };
    public float maxVol = 0.8f;

    //for the start cutscene
    public PlayerControls controls;
    public KeyboardControl keyboard;
    public GamepadControl gamepad;
    public CountdownTimer timeTracker;
    public AudioSource ambience;
    public GameObject blackScreen;
    public AudioSource carStart;
    public TextAsset markersFile;

    float numberOfLanes = 3;
    float laneWidth = 1.8f;
    float roadWidth = 1.8f * 3;

    //this is public so dialogue rewinding scripts know where to rewind too.
    public float currentDialogueStartTime = 0.0f;
    //this is public so error checking knows how far the player got
    public float endOfLevel = 1.0f;

    bool skipSection = false;
    bool skipIntro = false;
    void parseMarkersFromTextFile()
    {
        StreamReader inp_stm = new StreamReader(Application.dataPath + "/" + "Level1.txt");

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            // Do Something with the input. 
        }

        inp_stm.Close();
    }

    void parseLevelMarkers()
    {
        timedObstacleMarkers = new List<string>();
        dialogueMarkers = new List<string>();
        spawnedObstacles = new Dictionary<GameObject, float>();
        levelDialogue = GetComponent<AudioSource>();

        string[] lines = markersFile.text.Split('\n');

        int lineNumber = 1;
        foreach (string line in lines)
        {
            string[] tokens = line.Split(new char[] { ' ', '\t' });
            if (tokens.Length < 3 || !char.IsDigit(tokens[0][0])) continue;
            float previousStartTime = 0;
            bool newTrack = false;
            float startTime = float.Parse(tokens[0]);
            int lineLength;
            //markers will either be obstacles/dialogue, or news/realtime events
            if (tokens.Length == 3)
            {
                if (tokens[2][0] == '[')
                {
                    print("parsed command marker" + tokens.Length);
                    commandMarkers.Add(tokens[0] + "-" + tokens[1] + "-" + tokens[2]);
                }
                else
                {
                    dialogueMarkers.Add(tokens[0] + "-" + tokens[1] + "-" + tokens[2]);
                }
            } else if (startTime >= previousStartTime && !newTrack)
            {
                timedObstacleMarkers.Add(tokens[0] + "-" + tokens[1] + "-" + string.Join(" ", tokens, 2, tokens.Length - 2));
                previousStartTime = startTime;
            } else
            {
                newTrack = true;
                distanceObstacleMarkers.Add(tokens[0] + "-" + tokens[1] + "-" + string.Join(" ", tokens, 2, tokens.Length - 2));
            }
            //print("amount of tokens are " + tokens.Length);
            lineNumber++;
        }
    }

    [ContextMenu("Construct Level")]
    void constructLevel()
    {
        parseLevelMarkers();

        constructLevelMap();
    }

    void Start()
    {
        parseLevelMarkers();

        if (GameObject.Find("Map"))
        {
            Destroy(GameObject.Find("Map"));

            constructLevelMap();

            GameObject.Find("Map").name = "NewerMap";
        }

        StartCoroutine(lockWheel());
        StartCoroutine(shiftLoopSectionOfMusic(16f, 70.5f));
        StartCoroutine(playLevel());
    }

    void constructLevelMap()
    {
        float updateRate = 50; //how long fixedupdate runs per second

        GameObject GPS = Resources.Load<GameObject>("GPSMarker");
        GameObject curb = Resources.Load<GameObject>("Curb");
        GameObject road = Resources.Load<GameObject>("Road");

        GameObject map = new GameObject("Map");
        float length = levelDialogue.clip.length * controls.neutralSpeed * updateRate;
        GameObject roadtile = Instantiate(road, new Vector3(0, 0, 1), Quaternion.identity);
        roadtile.transform.localScale = new Vector3(roadWidth, length, 1);
        roadtile.transform.parent = map.transform;
        GameObject leftcurb = Instantiate(curb, new Vector3(-roadWidth/2 - 0.5f, 0, 1), Quaternion.identity);
        leftcurb.transform.localScale = new Vector3(1,length,1);
        leftcurb.transform.parent = map.transform;
        GameObject rightcurb = Instantiate(curb, new Vector3(roadWidth/2 + 0.5f, 0, 1), Quaternion.identity);
        rightcurb.transform.localScale = new Vector3(1, length, 1);
        rightcurb.transform.parent = map.transform;
        playerTransform.position = new Vector3(0, -length / 2, 0);

        GameObject GPSstart = Instantiate(GPS, new Vector3(0, -length/2, 1), Quaternion.identity);
        GPSstart.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/gps_start");
        GPSstart.transform.parent = map.transform;

        //GameObject dialogueZone = Resources.Load<GameObject>("DialogueMarker");
    }

    IEnumerator playLevel()
    {
        bool midpoint = false;
        bool endpoint = false;
        print("starting level now");
        int updateRate = 50;
        endOfLevel = float.Parse(dialogueMarkers[dialogueMarkers.Count - 1].Split('-')[0]);
        print("level ends at " + endOfLevel);

        //perform these checks every frame for as long as the dialogue plays
        while (levelDialogue.time < endOfLevel)
        {
            //figure out when the current dialogue section ends and the next starts
            float currentDialogueEndTime = endOfLevel;
            float nextDialogueStartTime = endOfLevel;
            bool start = false;
            currentDialogueStartTime = 0.0f;

            if (dialogueMarkers.Count > 1)
            {
                currentDialogueStartTime = levelDialogue.time;
                currentDialogueEndTime = float.Parse(dialogueMarkers[0].Split('-')[1]);
                nextDialogueStartTime = float.Parse(dialogueMarkers[1].Split('-')[0]);
                if (string.Equals(dialogueMarkers[0].Split('-')[2].Trim(), "Start")) start = true;
                dialogueMarkers.RemoveAt(0);
            }

            //figure out when the next obstacle will spawn
            float nextObstacleSpawnTime = endOfLevel;
            if (timedObstacleMarkers.Count > 0)
            {
                nextObstacleSpawnTime = endOfLevel;
            }

            //places GPS markers at the middle and end of the dialogue
            if (levelDialogue.time >= endOfLevel / 2 && !midpoint)
            {
                secondSource.clip = Resources.Load<AudioClip>("Audio/gps_middle");
                secondSource.Play();
                yield return new WaitForSeconds(secondSource.clip.length);
                midpoint = true;
            }

            //create a physical marker that must be hit before the next piece of dialogue can play
            GameObject nextDialogueTrigger = Instantiate(Resources.Load<GameObject>("DisposableTrigger"), playerTransform.position + new Vector3(0, (nextDialogueStartTime - levelDialogue.time) * controls.neutralSpeed * updateRate, 1), Quaternion.identity);
            if (start)
            {
                nextDialogueTrigger = Instantiate(Resources.Load<GameObject>("DisposableTrigger"), playerTransform.position + new Vector3(0, (nextDialogueStartTime - currentDialogueEndTime) * controls.neutralSpeed * updateRate, 1), Quaternion.identity);
            }

            //start playing the dialogue from wherever it left off
            print("starting new dialogue section");
            levelDialogue.Play();
            isSpeaking = true;

            print("current time" + levelDialogue.time + "dialogue end" + currentDialogueEndTime + " next dialogue start " + nextDialogueStartTime);
            //while waiting for the next piece of dialogue, check if any obstacles need to be spawned or despawned, then remove from the queue. checks every frame
            while (levelDialogue.time < nextDialogueStartTime)
            {
                //print("time in the dialogue is " + levelDialogue.time);
                yield return new WaitForSeconds(0);

                //check list of markers to see if the next obstacle is due
                if (commandMarkers.Count > 0)
                {
                    string[] commandData = commandMarkers[0].Split('-');
                    string command = commandData[2].Trim();
                    float spawnTime = float.Parse(commandData[0]);
                    print("parsing command... " + command);
                    if (levelDialogue.time >= spawnTime)
                    {
                        AudioClip radioClip = Resources.Load<AudioClip>("Audio/" + command);
                        print("trying to find " + "Audio/" + command);
                        if (radioClip != null)
                        {
                            secondSource.clip = radioClip;
                            secondSource.Play();
                        } 
                        else if (string.Equals(command, "[RevealScreen]"))
                        {
                            blackScreen.SetActive(false);
                        }
                        else if (string.Equals(command, "[StartCar]"))
                        {
                            StartCoroutine(startCar());
                            yield return new WaitForSeconds(2);
                        }
                        commandMarkers.RemoveAt(0);
                    }
                }

                //check list of markers to see if the next obstacle is due
                if (timedObstacleMarkers.Count > 0)
                {
                    string[] obstacleData = timedObstacleMarkers[0].Split('-');
                    float spawnTime = float.Parse(obstacleData[0]);
                    float despawnTime = float.Parse(obstacleData[1]);

                    //if the next obstacle is due or if the obstacle trigger was touched, spawn it
                    if (spawnTime < nextDialogueStartTime && levelDialogue.time > spawnTime)
                    {
                        string[] obstacleSeq = obstacleData[2].Split(',');
                        print("spawning obstacles" + spawnTime);
                        foreach (string obstacle in obstacleSeq)
                        {
                            //instantiate the obstacles plotted at this time

                            //print(obstacle);
                            string[] tokens = obstacle.Trim().Split(new char[] { ' ', '\t' });
                            float spawnDistance = 8;
                            if (tokens.Length == 2)
                            {
                                GameObject obj = Instantiate(Resources.Load<GameObject>(tokens[0].Trim()),
                                    new Vector3(playerTransform.position.x, playerTransform.position.y + spawnDistance, 0),
                                    Quaternion.identity);
                                spawnedObstacles.Add(obj, despawnTime);
                                if (tokens[1].ToLower()[0] == 'r')
                                {
                                    obj.GetComponent<QuickTurn>().mustTurnLeft = false;
                                } else
                                {
                                    obj.GetComponent<QuickTurn>().mustTurnLeft = true;
                                }
                            }
                            else {
                                float xpos = tokens[2].ToLower()[0] == 'l' ? (-roadWidth + laneWidth) / 2 + (laneWidth * (float.Parse(tokens[2].Substring(4)) - 1)) :
                                    tokens[2].ToLower()[0] == 'r' ? (-roadWidth + laneWidth) / 2 + (laneWidth * Random.Range(0, numberOfLanes)) :
                                    tokens[2].ToLower().Trim() == "playersleft" && playerTransform.position.x > (-roadWidth + laneWidth) / 2 ? playerTransform.position.x - laneWidth :
                                    tokens[2].ToLower().Trim() == "playersright" && playerTransform.position.x < (roadWidth + laneWidth) / 2 ? playerTransform.position.x + laneWidth :
                                    playerTransform.position.x;
                                float ypos = playerTransform.position.y + (tokens[1].ToLower()[0] == 'a' || tokens[1].ToLower()[0] == 'f' ? spawnDistance : -spawnDistance);
                                print(tokens[0].Trim());
                                spawnedObstacles.Add(Instantiate(Resources.Load<GameObject>(tokens[0].Trim()),
                                    new Vector3(xpos, ypos, 0),
                                    Quaternion.identity), despawnTime);
                            }
                        }
                        timedObstacleMarkers.RemoveAt(0);
                    }
                }

                //check all active obstacles to see if any should be despawned
                foreach (KeyValuePair<GameObject, float> pair in spawnedObstacles)
                {
                    if (levelDialogue.time >= pair.Value)
                    {
                        GameObject obj = pair.Key;

                        obj.GetComponent<BoxCollider2D>().isTrigger = true;
                        if (obj.transform.position.x > playerTransform.position.x)
                            obj.transform.Rotate(0, 0, -90);
                        else
                            obj.transform.Rotate(0, 0, 90);
                        Destroy(pair.Key, 5);

                        spawnedObstacles.Remove(obj);
                        break;
                    }
                    //obstacles can spawn prematurely, but not despawn prematurely
                }

                //for debugging
                if (skipSection)
                {
                    Destroy(nextDialogueTrigger);
                    foreach (KeyValuePair<GameObject, float> pair in spawnedObstacles)
                    {
                        Destroy(pair.Key);
                    }
                    spawnedObstacles.Clear();

                    levelDialogue.time = nextDialogueStartTime;
                    break;
                }

                if (levelDialogue.time >= currentDialogueEndTime && nextDialogueTrigger == null && (timedObstacleMarkers.Count == 0 || (float.Parse(timedObstacleMarkers[0].Split('-')[0]) >= nextDialogueStartTime))) { break; }
            }

            print("finished section of dialogue");
            levelDialogue.Pause();
            isSpeaking = false;

            //wait until the next dialogue trigger is touched
            while (nextDialogueTrigger != null) { yield return new WaitForSeconds(0); }

            skipSection = false;
        }

        secondSource.clip = Resources.Load<AudioClip>("Audio/gps_end");
        secondSource.Play();
        yield return new WaitForSeconds(secondSource.clip.length);
        //This is where the level ends
        ScoreStorage.Instance.setScoreAll();
        SceneManager.LoadScene("EndScreen", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
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
        keyboard.enabled = true;
        gamepad.enabled = true;
        timeTracker.enabled = true;
        adjustInstrumentVolume(false, new string[] { });
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
            skipSection = true;
        }
        if (Input.GetKeyDown("l"))
        {
            skipIntro = true;
        }
    }

    // Changes the volume of individual instruments (currently unused)
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
                        child.gameObject.GetComponent<AudioSource>().volume = 0.5f;
                    }
                    else
                    {
                        child.gameObject.GetComponent<AudioSource>().volume = 1f;
                    }
                }
            }
        }
    }
}