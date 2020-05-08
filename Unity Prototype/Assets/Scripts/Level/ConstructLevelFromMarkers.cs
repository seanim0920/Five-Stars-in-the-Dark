using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ConstructLevelFromMarkers : MonoBehaviour
{
    public static AudioSource levelDialogue;
    public AudioSource secondSource;
    private GameObject player;

    private AudioClip carStart;
    private AudioClip carPark;

    List<string> timedObstacleMarkers = new List<string>();
    List<string> commandMarkers = new List<string>();
    List<string> distanceObstacleMarkers = new List<string>();
    List<string> dialogueMarkers = new List<string>();
    List<string> subtitleMarkers = new List<string>();
    Dictionary<GameObject, float> spawnedObstacles = new Dictionary<GameObject, float>();
    
    //variables set from the player object
    private SteeringWheelControl wheelFunctions;
    private PlayerControls controls;
    private SteeringWheelControl wheelControl;
    private KeyboardControl keyboard;
    private GamepadControl gamepad;
    private static int controlType; // 0 = Steering Wheel
                                    // 1 = Keyboard
                                    // 2 = Gamepad

    //for lowering the volume when dialogue is playing
    //public Transform leftSpeaker;
    //public Transform rightSpeaker;
    public static bool isSpeaking;
    private string[] dialogueInstruments = { "Drums", "Support", "Wind" };
    public float maxVol = 0.8f;

    //for the start cutscene
    public CountdownTimer timeTracker;
    public AudioSource ambience;
    public Image blackScreen;
    public TextAsset markersFile;
    public static string debugMessage { get; set; }
    public static string subtitleMessage { get; set; }

    float numberOfLanes = 3;
    float laneWidth = 1.8f * 20;
    float roadWidth = 1.8f * 3 * 20;

    //this is public so dialogue rewinding scripts know where to rewind too.
    public float currentDialogueStartTime = 0.0f;
    //this is public so error checking knows how far the player got
    public float startOfLevel = 0f;
    public float endOfLevel = 1.0f;

    bool skipSection = false;
    bool skipIntro = false;

    Object[] loadedObjects;
    void parseMarkersFromTextFile()
    {
        timedObstacleMarkers = new List<string>();
        dialogueMarkers = new List<string>();
        spawnedObstacles = new Dictionary<GameObject, float>();
        levelDialogue = GetComponent<AudioSource>();

        StreamReader inp_stm = new StreamReader(Application.dataPath + "/" + "Level.txt");

        int lineNumber = 1;
        while (!inp_stm.EndOfStream)
        {
            string line = inp_stm.ReadLine();

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
                    if (string.Equals(tokens[2].Trim(), "[StartCar]"))
                    {
                        startOfLevel = float.Parse(tokens[0]);
                    }
                    commandMarkers.Add(tokens[0] + "-" + tokens[1] + "-" + tokens[2]);
                }
                else
                {
                    dialogueMarkers.Add(tokens[0] + "-" + tokens[1] + "-" + tokens[2]);
                }
            }
            else
            {
                if (tokens[2][0] == '"')
                {
                    subtitleMarkers.Add(tokens[0] + "-" + tokens[1] + "-" + string.Join(" ", tokens, 2, tokens.Length - 2));
                }
                timedObstacleMarkers.Add(tokens[0] + "-" + tokens[1] + "-" + string.Join(" ", tokens, 2, tokens.Length - 2));
                previousStartTime = startTime;
            }
            //print("amount of tokens are " + tokens.Length);
            lineNumber++;
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
            } else
            {
                if (tokens[2][0] == '"')
                {
                    print("parsed subtitle marker" + tokens.Length);
                    subtitleMarkers.Add(tokens[0] + "-" + tokens[1] + "-" + string.Join(" ", tokens, 2, tokens.Length - 2));
                } else
                {
                    timedObstacleMarkers.Add(tokens[0] + "-" + tokens[1] + "-" + string.Join(" ", tokens, 2, tokens.Length - 2));
                }
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
        loadedObjects = Resources.LoadAll("Prefabs/Obstacles", typeof(GameObject));
        foreach (var go in loadedObjects)
        {
            Debug.Log(go.name);
        }
        player = GameObject.Find("Player");
        wheelFunctions = player.GetComponent<SteeringWheelControl>();
        controls = player.GetComponent<PlayerControls>();
        wheelControl = player.GetComponent<SteeringWheelControl>();
        keyboard = player.GetComponent<KeyboardControl>();
        gamepad = player.GetComponent<GamepadControl>();
        parseLevelMarkers();

        carStart = Resources.Load<AudioClip>("Audio/Car-SFX/Car Ambience/Car-EngineStart");
        carPark = Resources.Load<AudioClip>("Audio/Car-SFX/Car Ambience/Car-EngineStart");

        /*
        if (GameObject.Find("Map"))
        {
            Destroy(GameObject.Find("Map"));

            constructLevelMap();

            GameObject.Find("Map").name = "NewerMap";
        }
        */

        StartCoroutine(lockWheel());
        StartCoroutine(shiftLoopSectionOfMusic(16f, 70.5f));
        StartCoroutine(playLevel());
    }

    void constructLevelMap()
    {
        float updateRate = 50; //how long fixedupdate runs per second

        GameObject curb = Resources.Load<GameObject>("Prefabs/Curb");
        GameObject road = Resources.Load<GameObject>("Prefabs/Road");

        GameObject map = new GameObject("Map");
        float length = levelDialogue.clip.length * controls.neutralSpeed * updateRate;
        GameObject roadtile = Instantiate(road, new Vector3(0, 0, 1), Quaternion.identity);
        roadtile.transform.localScale = new Vector3(roadWidth, length, 1);
        roadtile.transform.parent = map.transform;
        GameObject leftcurb = Instantiate(curb, new Vector3((-roadWidth/2 - 0.5f), 0, 1), Quaternion.identity);
        leftcurb.transform.localScale = new Vector3(20,length,1);
        leftcurb.transform.parent = map.transform;
        GameObject rightcurb = Instantiate(curb, new Vector3((roadWidth/2 + 0.5f), 0, 1), Quaternion.identity);
        rightcurb.transform.localScale = new Vector3(20, length, 1);
        rightcurb.transform.parent = map.transform;
        player.transform.position = new Vector3(0, -length / 2, 0);
    }

    IEnumerator playLevel()
    {
        bool midpoint = false;
        bool endpoint = false;
        debugMessage = "starting level now, level ends at " + endOfLevel;
        subtitleMessage = "";
        int updateRate = 50;
        endOfLevel = float.Parse(dialogueMarkers[dialogueMarkers.Count - 1].Split('-')[0]);

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
            if (levelDialogue.time + startOfLevel >= (endOfLevel - startOfLevel) / 2 && !midpoint)
            {
                print(levelDialogue.time + "level ends at " + endOfLevel);
                //secondSource.clip = Resources.Load<AudioClip>("Audio/Car-SFX/GPS Library/gps_middle");
                //secondSource.Play();
                //yield return new WaitForSeconds(secondSource.clip.length);
                midpoint = true;
            }

            //create a physical marker that must be hit before the next piece of dialogue can play
            GameObject nextDialogueTrigger = Instantiate(Resources.Load<GameObject>("Prefabs/DisposableTrigger"), player.transform.position + new Vector3(0, (nextDialogueStartTime - levelDialogue.time) * controls.neutralSpeed * updateRate, 1), Quaternion.identity);
            if (start)
            {
                nextDialogueTrigger = Instantiate(Resources.Load<GameObject>("Prefabs/DisposableTrigger"), player.transform.position + new Vector3(0, (nextDialogueStartTime - currentDialogueEndTime) * controls.neutralSpeed * updateRate, 1), Quaternion.identity);
            }

            //start playing the dialogue from wherever it left off
            levelDialogue.Play();
            isSpeaking = true;

            debugMessage = "starting new dialogue section: " + "ends at " + currentDialogueEndTime + " next dialogue starts at " + nextDialogueStartTime;
            //while waiting for the next piece of dialogue, check if any obstacles need to be spawned or despawned, then remove from the queue. checks every frame
            while (levelDialogue.time < nextDialogueStartTime)
            {
                //print("time in the dialogue is " + levelDialogue.time);
                yield return new WaitForSeconds(0);
                debugMessage = "current time in dialogue: " + levelDialogue.time + "... ";

                //check list of markers to see if the next obstacle is due
                if (commandMarkers.Count > 0)
                {
                    string[] commandData = commandMarkers[0].Split('-');
                    string command = commandData[2].Trim();
                    float spawnTime = float.Parse(commandData[0]);
                    if (levelDialogue.time >= spawnTime)
                    {
                        debugMessage += "parsing command: " + command;
                        AudioClip radioClip = Resources.Load<AudioClip>(SceneManager.GetActiveScene().name + "/" + command);
                        if (radioClip != null)
                        {
                            secondSource.clip = radioClip;
                            secondSource.Play();
                        } 
                        else if (string.Equals(command, "[RevealScreen]"))
                        {
                            blackScreen.enabled = false;
                        }
                        else if (string.Equals(command, "[HideScreen]"))
                        {
                            blackScreen.enabled = true;
                        }
                        else if (string.Equals(command, "[StartCar]"))
                        {
                            print("started car at time " + levelDialogue.time);
                            StartCoroutine(startCar());
                            yield return new WaitForSeconds(2);
                            levelDialogue.Pause();
                            //secondSource.clip = Resources.Load<AudioClip>("Audio/Car-SFX/GPS Library/gps_start");
                            //secondSource.Play();
                            //yield return new WaitForSeconds(secondSource.clip.length);
                            levelDialogue.Play();
                        }
                        else if (string.Equals(command, "[EndControl]"))
                        {
                            StartCoroutine(parkCar());
                        }
                        commandMarkers.RemoveAt(0);
                    }
                }

                //check list of markers to see if the next subtitle is due
                if (subtitleMarkers.Count > 0)
                {
                    string[] subtitleData = subtitleMarkers[0].Split('-');
                    float spawnTime = float.Parse(subtitleData[0]);
                    float despawnTime = float.Parse(subtitleData[1]);

                    //if the next obstacle is due or if the obstacle trigger was touched, spawn it
                    if (levelDialogue.time >= spawnTime)
                    {
                        debugMessage += "printing subtitles: " + subtitleData[2];

                        subtitleMessage = subtitleData[2];
                    } else if (levelDialogue.time > despawnTime)
                    {
                        subtitleMessage = "";
                        subtitleMarkers.RemoveAt(0);
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
                        debugMessage += "spawning obstacles: " + obstacleData[2];
                        string[] obstacleSeq = obstacleData[2].Split(',');
                        foreach (string obstacle in obstacleSeq)
                        {
                            //instantiate the obstacles plotted at this time

                            string[] tokens = obstacle.Trim().Split(new char[] { ' ', '\t' });
                            float spawnDistance = 200;

                            string prefab = "";
                            foreach (var obj in loadedObjects)
                            {
                                if (string.Equals(obj.name, tokens[0].Trim(), System.StringComparison.OrdinalIgnoreCase))
                                {
                                    Debug.Log("found obstacle");
                                    prefab = obj.name;
                                }
                            }
                            if (prefab == "") { Debug.Log("could not load obstacle"); break; }

                            if (tokens.Length == 2)
                            {
                                GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Obstacles/" + prefab),
                                    new Vector3(player.transform.position.x, player.transform.position.y + spawnDistance, 0),
                                    Quaternion.identity);
                                if (nextDialogueTrigger == null) //checks whether trigger was already hit, if so spawn another one and spawn it further ahead. not the best programming practice but itll do for now.
                                    nextDialogueTrigger = Instantiate(Resources.Load<GameObject>("Prefabs/DisposableTrigger"), player.transform.position + new Vector3(0, (nextDialogueStartTime - levelDialogue.time) * controls.neutralSpeed * updateRate, 1), Quaternion.identity);
                                if ((string.Equals(prefab, "quickturn", System.StringComparison.OrdinalIgnoreCase)))
                                {
                                    if ((string.Equals(tokens[1].Trim(), "right", System.StringComparison.OrdinalIgnoreCase)))
                                    {
                                        obj.GetComponent<QuickTurn>().mustTurnLeft = false;
                                    }
                                    else
                                    {
                                        obj.GetComponent<QuickTurn>().mustTurnLeft = true;
                                    }
                                    nextDialogueTrigger.transform.position += new Vector3(0,50,0);
                                } else if ((string.Equals(prefab, "stoplight", System.StringComparison.OrdinalIgnoreCase)))
                                {
                                    string pattern = tokens[1].ToLower().Trim();
                                    obj.GetComponent<Stoplight>().pattern = pattern;
                                    nextDialogueTrigger.transform.position += new Vector3(0, 350, 0); //I think this is the length of the stoplight object?
                                }
                            }
                            else {
                                float xpos = tokens[2].ToLower()[0] == 'l' ? (-roadWidth + laneWidth) / 2 + (laneWidth * (float.Parse(tokens[2].Substring(4)) - 1)) :
                                    tokens[2].ToLower()[0] == 'r' ? (-roadWidth + laneWidth) / 2 + (laneWidth * Random.Range(0, numberOfLanes)) :
                                    tokens[2].ToLower().Trim() == "playersleft" && player.transform.position.x > (-roadWidth + laneWidth) / 2 ? player.transform.position.x - laneWidth :
                                    tokens[2].ToLower().Trim() == "playersright" && player.transform.position.x < (roadWidth + laneWidth) / 2 ? player.transform.position.x + laneWidth :
                                    player.transform.position.x;
                                float ypos = player.transform.position.y + (tokens[1].ToLower()[0] == 'a' || tokens[1].ToLower()[0] == 'f' ? spawnDistance : -spawnDistance);
                                //print(tokens[0].Trim());
                                spawnedObstacles.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Obstacles/"+prefab),
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
                        debugMessage += "despawning obstacle: " + obj.name;

                        obj.GetComponent<CapsuleCollider2D>().isTrigger = true;
                        if (obj.transform.position.x > player.transform.position.x)
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

            debugMessage = "finished section of dialogue";
            levelDialogue.Pause();
            isSpeaking = false;

            //wait until the next dialogue trigger is touched
            while (nextDialogueTrigger != null) { yield return new WaitForSeconds(0); }

            skipSection = false;
        }

        //This is where the level ends
        //secondSource.clip = Resources.Load<AudioClip>("Audio/Car-SFX/GPS Library/gps_end");
        //secondSource.Play();
        //yield return new WaitForSeconds(secondSource.clip.length);
        levelDialogue.Play();
        while (levelDialogue.isPlaying)
        {
            yield return new WaitForSeconds(0);
        }

        ScoreStorage.Instance.setScoreAll();
        SceneManager.LoadScene("EndScreen", LoadSceneMode.Single);
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }

    IEnumerator startCar()
    {
        blackScreen.enabled = false;
        ambience.Play();
        yield return new WaitForSeconds(1);
        secondSource.PlayOneShot(carStart);
        StartCoroutine(wheelRumble());
        yield return new WaitForSeconds(1);
        controls.enabled = true;
        Debug.Log(controlType);
        if(controlType == 0 && (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0)))
        {
            wheelControl.enabled = true;
        }
        else if(controlType == 2 && Gamepad.current != null)
        {
            gamepad.enabled = true;
        }
        else
        {
            keyboard.enabled = true;
        }
        timeTracker.enabled = true;
        adjustInstrumentVolume(false, new string[] { });
    }
    IEnumerator parkCar()
    {
        keyboard.enabled = false;
        gamepad.enabled = false;
        while (Mathf.Abs(controls.movementSpeed) > 0.01f)
        {
            controls.movementSpeed *= 0.97f;
            yield return new WaitForSeconds(0);
        }
        controls.movementSpeed = 0;
        controls.enabled = false;
        timeTracker.enabled = false;
        secondSource.PlayOneShot(carPark);
        yield return new WaitForSeconds(1);
        blackScreen.GetComponent<Image>().CrossFadeAlpha(0, 3.0f, false);
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
        //while (true)
        //{
        //    for (int loop = 0; loop < 2; loop++)
        //    {
        //        Transform speaker = leftSpeaker;
        //        if (loop > 0) speaker = rightSpeaker;
        //        foreach (Transform child in speaker)
        //        {
        //            AudioSource instrument = child.gameObject.GetComponent<AudioSource>();
        //            if (instrument.time >= endTime)
        //            {
        //                child.gameObject.GetComponent<AudioSource>().time = startTime;
        //            }
        //        }
        //    }
        //    yield return null;
        //}
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s") || Gamepad.current.buttonSouth.isPressed)
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
        //isSpeaking = dialogueStart;
        //for (int loop = 0; loop < 2; loop++)
        //{
        //    Transform speaker = leftSpeaker;
        //    if (loop > 0) speaker = rightSpeaker;
        //    foreach (Transform child in speaker)
        //    {
        //        if (instruments.Length == 0 || System.Array.IndexOf(instruments, child.gameObject.name) > -1)
        //        {
        //            if (dialogueStart)
        //            {
        //                child.gameObject.GetComponent<AudioSource>().volume = 0.5f;
        //            }
        //            else
        //            {
        //                child.gameObject.GetComponent<AudioSource>().volume = 1f;
        //            }
        //        }
        //    }
        //}
    }

    public void setController(int type)
    {
        Debug.Log("controller type: " + type);
        controlType = type;
    }
}