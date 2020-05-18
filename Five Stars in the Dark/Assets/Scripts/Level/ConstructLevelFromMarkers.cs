﻿using System.Collections;
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

    List<Marker> timedObstacleMarkers = new List<Marker>();
    List<Marker> commandMarkers = new List<Marker>();
    List<Marker> dialogueMarkers = new List<Marker>();
    List<Marker> subtitleMarkers = new List<Marker>();
    Dictionary<GameObject, float> spawnedObstacles = new Dictionary<GameObject, float>();
    
    //variables set from the player object
    private SteeringWheelInput wheelFunctions;
    private PlayerControls controls;
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
    public AudioSource ambience;
    public Image blackScreen;
    public TextAsset markersFile;
    public static string debugMessage { get; set; }
    public static string subtitleMessage { get; set; } = "";

    float numberOfLanes = 3;
    float laneWidth = 1.8f * 20;
    float roadWidth = 1.8f * 3 * 20;

    //this is public so dialogue rewinding scripts know where to rewind too.
    public float currentDialogueStartTime = 0.0f;
    //this is public so error checking knows how far the player got
    private static float startOfLevel = 0f;
    private static float endOfLevel = 0f;

    private char firstDelimiter = ';';

    bool skipSection = false;
    bool skipIntro = false;

    GameObject nextDialogueTrigger;
    GameObject dialogueStopper;

    Object[] loadedObjects;

    struct Marker
    {
        public float spawnTime;
        public float despawnTime;
        public string data;
        public Marker(float spawnTime, float despawnTime, string data)
        {
            this.spawnTime = spawnTime;
            this.despawnTime = despawnTime;
            this.data = data;
        }
    }

    void sortedMarkerInsert(List<Marker> list, Marker newmarker)
    {
        int i = 0;
        for (i = 0; i < list.Count; i++)
        {
            if (newmarker.spawnTime < list[i].spawnTime)
            {
                break;
            }
        }
        list.Insert(i, newmarker);
    }

    public static float getProgress()
    {
        return (levelDialogue.time - startOfLevel) / (endOfLevel - startOfLevel) * 100;
    }

    void parseLevelMarkers()
    {
        timedObstacleMarkers = new List<Marker>();
        dialogueMarkers = new List<Marker>();
        spawnedObstacles = new Dictionary<GameObject, float>();
        levelDialogue = GetComponent<AudioSource>();

        string[] lines = markersFile.text.Split('\n');

        int lineNumber = 1;
        foreach (string line in lines)
        {
            string[] tokens = line.Split(new char[] { ' ', '\t' });
            if (tokens.Length < 3 || !char.IsDigit(tokens[0][0])) continue;
            float startTime = float.Parse(tokens[0]);
            //markers will either be obstacles/dialogue, or news/realtime events
            if (tokens.Length == 3)
            {
                Marker newMarker = new Marker(float.Parse(tokens[0].Trim()), float.Parse(tokens[1].Trim()), tokens[2].Trim());
                if (tokens[2][0] == '[')
                {
                    sortedMarkerInsert(commandMarkers, newMarker);
                    if (string.Equals(newMarker.data, "[StartControl]") || string.Equals(newMarker.data, "[StartCar]"))
                    {
                        startOfLevel = newMarker.spawnTime;
                    }
                    if (string.Equals(newMarker.data, "[EndControl]"))
                    {
                        endOfLevel = newMarker.spawnTime;
                    }
                }
                else if (tokens[2].ToLower()[0] == 'd')
                {
                    sortedMarkerInsert(dialogueMarkers, newMarker);
                }
            } else
            {
                Marker newMarker = new Marker(float.Parse(tokens[0].Trim()), float.Parse(tokens[1].Trim()), string.Join(" ", tokens, 2, tokens.Length - 2));
                if (tokens[2][0] == '"' || tokens[2][0] == '<')
                {
                    sortedMarkerInsert(subtitleMarkers, newMarker);
                } else
                {
                    sortedMarkerInsert(timedObstacleMarkers, newMarker);
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
        ScoreStorage.Instance.resetScore();
        loadedObjects = Resources.LoadAll("Prefabs/Obstacles", typeof(GameObject));
        player = GameObject.Find("Player");
        wheelFunctions = player.GetComponent<SteeringWheelInput>();
        controls = player.GetComponent<PlayerControls>();
        keyboard = player.GetComponent<KeyboardControl>();
        gamepad = player.GetComponent<GamepadControl>();
        parseLevelMarkers();

        if (!blackScreen.enabled)
        {
            enableControllers();
        } else
        {
            disableControllers();
        }

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

    public void enableControllers()
    {
        if (SettingsManager.toggles[0])
        {
            gamepad.enabled = false;
            keyboard.enabled = false;
            wheelFunctions.enabled = true;
        }
        else if (SettingsManager.toggles[2])
        {
            wheelFunctions.enabled = false;
            keyboard.enabled = false;
            gamepad.enabled = true;
        }
        else
        {
            wheelFunctions.enabled = false;
            gamepad.enabled = false;
            keyboard.enabled = true;
        }
    }

    void disableControllers()
    {
        wheelFunctions.enabled = false;
        gamepad.enabled = false;
        keyboard.enabled = false;
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
        debugMessage = "starting level now, level ends at " + endOfLevel;
        subtitleMessage = "";
        int updateRate = 50;
        if (endOfLevel == 0)
        {
            endOfLevel = levelDialogue.clip.length;
        }
        //initial parsing of the theoretical fastest level time, for the sake of score calculation
        //should subtract startleveltime from endleveltime
        ScoreStorage.Instance.setScorePar((int)endOfLevel * 100);

        print("dialoguemarkers.count " + dialogueMarkers.Count + timedObstacleMarkers.Count);

        //perform these checks every frame for as long as the dialogue plays
        while (dialogueMarkers.Count > 0 || timedObstacleMarkers.Count > 0)
        {
            print("checking markers?");
            //figure out when the current dialogue section ends and the next starts
            float currentDialogueEndTime = levelDialogue.clip.length;
            float nextDialogueStartTime = levelDialogue.clip.length;
            currentDialogueStartTime = 0.0f;

            if (dialogueMarkers.Count > 1)
            {
                currentDialogueStartTime = levelDialogue.time;
                currentDialogueEndTime = dialogueMarkers[0].despawnTime;
                nextDialogueStartTime = dialogueMarkers[1].spawnTime;
            }
            if (dialogueMarkers.Count > 0)
            {
                dialogueMarkers.RemoveAt(0);
            }

            //create a physical marker that must be hit before the next piece of dialogue can play
            nextDialogueTrigger = Instantiate(Resources.Load<GameObject>("Prefabs/DisposableTrigger"), player.transform.position + new Vector3(0, (nextDialogueStartTime - levelDialogue.time) * controls.neutralSpeed * updateRate, 1), Quaternion.identity);

            //start playing the dialogue from wherever it left off
            levelDialogue.Play();
            isSpeaking = true;

            debugMessage = "starting new dialogue section: " + "ends at " + currentDialogueEndTime + " next dialogue starts at " + nextDialogueStartTime;
            //while waiting for the next piece of dialogue, check if any obstacles need to be spawned or despawned, then remove from the queue. checks every frame
            while ((nextDialogueTrigger != null && (levelDialogue.time < nextDialogueStartTime && levelDialogue.isPlaying)) || commandMarkers.Count > 0)
            {
                //if nextdialoguetrigger disappears before the current section is finished, the current section will repeat. bug.
                //print("time in the dialogue is " + levelDialogue.time);
                yield return new WaitForSeconds(0);
                debugMessage = "current time in dialogue: " + levelDialogue.time + "... ";

                if (dialogueStopper != null && !dialogueStopper.CompareTag("Car"))
                {
                    while (dialogueStopper != null && !dialogueStopper.CompareTag("Car") && !skipSection)
                    {
                        yield return new WaitForSeconds(0);
                    }
                    if (dialogueStopper != null && !dialogueStopper.CompareTag("Car")) Destroy(dialogueStopper);
                    controls.enabled = true;
                    enableControllers();
                    if (nextDialogueTrigger != null)
                    {
                        Destroy(nextDialogueTrigger);
                    }
                    nextDialogueTrigger = Instantiate(Resources.Load<GameObject>("Prefabs/DisposableTrigger"), player.transform.position + new Vector3(0, (nextDialogueStartTime - levelDialogue.time) * controls.neutralSpeed * updateRate, 1), Quaternion.identity);
                    levelDialogue.Play();
                }

                //check list of markers to see if the next obstacle is due
                if (commandMarkers.Count > 0)
                {
                    Marker commandMarker = commandMarkers[0];
                    string command = commandMarker.data;
                    if (levelDialogue.time >= commandMarker.spawnTime)
                    {
                        debugMessage += "parsing command: " + command;
                        AudioClip radioClip = Resources.Load<AudioClip>(SceneManager.GetActiveScene().name + "/" + command.Trim('[', ']'));
                        print("Looking for audiofile" + SceneManager.GetActiveScene().name + "/" + command.Trim('[', ']'));
                        if (radioClip != null)
                        {
                            secondSource.clip = radioClip;
                            secondSource.Play();
                        } 
                        else if (string.Equals(command, "[RevealScreen]"))
                        {
                            enableControllers();
                            blackScreen.enabled = false;
                        }
                        else if (string.Equals(command, "[HideScreen]"))
                        {
                            disableControllers();
                            blackScreen.enabled = true;
                        }
                        else if (string.Equals(command, "[StartCar]") || string.Equals(command, "[StartControl]"))
                        {
                            if (nextDialogueTrigger != null) Destroy(nextDialogueTrigger);
                            nextDialogueTrigger = Instantiate(Resources.Load<GameObject>("Prefabs/DisposableTrigger"), player.transform.position + new Vector3(0, (nextDialogueStartTime - levelDialogue.time) * controls.neutralSpeed * updateRate, 1), Quaternion.identity);
                            print("started car at time " + levelDialogue.time);
                            StartCoroutine(startCar());
                            yield return new WaitForSeconds(2);
                            levelDialogue.Pause();
                            levelDialogue.Play();
                        }
                        else if (string.Equals(command, "[EndControl]"))
                        {
                            print("ending player control");
                            StartCoroutine(parkCar());
                        }
                        commandMarkers.RemoveAt(0);
                    }
                }

                //check list of markers to see if the next subtitle is due
                updateSubtitle();

                //check list of markers to see if the next obstacle is due
                if (timedObstacleMarkers.Count > 0)
                {
                    Marker obstacleMarker = timedObstacleMarkers[0];
                    //print("trying to spawn obstacle at time " + spawnTime);

                    //if the next obstacle is due or if the obstacle trigger was touched, spawn it
                    if (obstacleMarker.spawnTime < nextDialogueStartTime && obstacleMarker.spawnTime < levelDialogue.time)
                    {
                        debugMessage += "spawning obstacles: " + obstacleMarker.data;
                        spawnObstacles(obstacleMarker.despawnTime, obstacleMarker.data);
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

                        if (obj.transform.position.x > player.transform.position.x)
                            obj.transform.Rotate(0, 0, -45);
                        else
                            obj.transform.Rotate(0, 0, 45);
                        if (pair.Key.GetComponent<NPCMovement>().neutralSpeed != 0)
                        {
                            obj.GetComponent<CapsuleCollider2D>().isTrigger = true;
                            Destroy(pair.Key, 5);
                        }

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

                    if (nextDialogueStartTime < levelDialogue.clip.length)
                        levelDialogue.time = nextDialogueStartTime;
                    else
                        levelDialogue.time = levelDialogue.clip.length;
                    break;
                }
            }

            print("finished section of dialogue, " + levelDialogue.isPlaying + dialogueMarkers.Count);
            if (!levelDialogue.isPlaying && dialogueMarkers.Count == 0) break;

            debugMessage = "finished section of dialogue";
            levelDialogue.Pause();
            isSpeaking = false;

            //wait until the next dialogue trigger is touched
            while (nextDialogueTrigger != null) { yield return new WaitForSeconds(0); }

            skipSection = false;
        }

        //This is where the level ends
        ScoreStorage.Instance.setScoreAll();
        MasterkeyEndScreen.currentLevel = SceneManager.GetActiveScene().name;
        ScoreStorage.Instance.setScoreProgress(100);
        SceneManager.LoadScene("EndScreen", LoadSceneMode.Single);
    }

    void spawnObstacles(float despawnTime, string obstacleData)
    {
        string[] obstacleSeq = obstacleData.Split(',');
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
                if (nextDialogueTrigger != null) //checks whether trigger was already hit, if so spawn another one and spawn it further ahead. not the best programming practice but itll do for now.
                    Destroy(nextDialogueTrigger);
                levelDialogue.Pause();
                print("paused dialogue here");
                if ((string.Equals(prefab, "quickturn", System.StringComparison.OrdinalIgnoreCase)))
                {
                    if (tokens.Length > 1)
                    {
                        if ((string.Equals(tokens[1].Trim(), "right", System.StringComparison.OrdinalIgnoreCase)))
                        {
                            obj.GetComponent<QuickTurn>().mustTurnLeft = false;
                        }
                        else
                        {
                            obj.GetComponent<QuickTurn>().mustTurnLeft = true;
                        }
                    }
                }
                else if ((string.Equals(prefab, "stoplight", System.StringComparison.OrdinalIgnoreCase)))
                {
                    if (tokens.Length > 1)
                    {
                        string pattern = tokens[1].ToLower().Trim();
                        obj.GetComponent<Stoplight>().pattern = pattern;
                    }
                }
                else if ((string.Equals(prefab, "target", System.StringComparison.OrdinalIgnoreCase)))
                {
                    if (tokens.Length > 1)
                    {
                        string pattern = tokens[1].ToLower().Trim();
                        obj.GetComponent<TargetMovement>().sequence = pattern;
                    }
                }
                dialogueStopper = obj;
            }
            else
            {
                float xpos = tokens[2].ToLower()[0] == 'l' ? (-roadWidth + laneWidth) / 2 + (laneWidth * (float.Parse(tokens[2].Substring(4)) - 1)) :
                    tokens[2].ToLower()[0] == 'r' ? (-roadWidth + laneWidth) / 2 + (laneWidth * Random.Range(0, numberOfLanes)) :
                    tokens[2].ToLower().Trim() == "playersleft" && player.transform.position.x > (-roadWidth + laneWidth) / 2 ? player.transform.position.x - laneWidth :
                    tokens[2].ToLower().Trim() == "playersright" && player.transform.position.x < (roadWidth + laneWidth) / 2 ? player.transform.position.x + laneWidth :
                    player.transform.position.x;
                float ypos = player.transform.position.y + (tokens[1].ToLower()[0] == 'a' || tokens[1].ToLower()[0] == 'f' ? spawnDistance : -spawnDistance);
                //print(tokens[0].Trim());
                spawnedObstacles.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Obstacles/" + prefab),
                    new Vector3(xpos, ypos, 0),
                    Quaternion.identity), despawnTime);
            }
        }
    }

    private void updateSubtitle()
    {
        if (subtitleMarkers.Count > 0)
        {
            Marker subtitleMarker = subtitleMarkers[0];

            //if the next obstacle is due or if the obstacle trigger was touched, spawn it
            if (levelDialogue.time >= subtitleMarker.spawnTime)
            {
                subtitleMessage = subtitleMarker.data;
                if (levelDialogue.time >= subtitleMarker.despawnTime)
                {
                    if (subtitleMarkers.Count > 1)
                    {
                        if (subtitleMarkers[1].spawnTime - subtitleMarker.despawnTime > 1)
                        {
                            subtitleMessage = "";
                        }
                    }
                    subtitleMarkers.RemoveAt(0);
                }
            }
        }
    }

    IEnumerator startCar()
    {
        blackScreen.enabled = false;
        ambience.Play();
        CountdownTimer.setTracking(true); //marks when the level is commanded to start
        yield return new WaitForSeconds(1);
        secondSource.PlayOneShot(carStart);
        StartCoroutine(wheelRumble());
        yield return new WaitForSeconds(1);
        controls.enabled = true;
        Debug.Log(controlType);
        CountdownTimer.decrementTime(2); //to make up for the two seconds took to start the engine
        adjustInstrumentVolume(false, new string[] { });
    }
    IEnumerator parkCar()
    {
        controls.parkCar();
        CountdownTimer.setTracking(false);
        secondSource.PlayOneShot(carPark);
        yield return new WaitForSeconds(1);
        //blackScreen.CrossFadeAlpha(0, 3.0f, false);
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
        if (Input.GetKeyDown("s") || (Gamepad.current != null && Gamepad.current.buttonEast.isPressed))
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
        // Debug.Log("controller type: " + type);
        // // controlType = type;
        // Debug.Log(" dialogue? " + (levelDialogue == null));
        if(levelDialogue != null)
        {
            enableControllers();
        }
    }

    public void toggleWheel(bool isActive)
    {
        SettingsManager.toggles[0] = isActive;
        SettingsManager.setToggles();
        if(isActive)
        {
            controlType = 0;
        }
        setController(controlType);
    }

    public void toggleKeyboard(bool isActive)
    {
        SettingsManager.toggles[1] = isActive;
        SettingsManager.setToggles();

        if(isActive)
        {
            controlType = 1;
        }
        setController(controlType);
    }

    public void toggleGamepad(bool isActive)
    {
        SettingsManager.toggles[2] = isActive;
        SettingsManager.setToggles();

        if(isActive)
        {
            controlType = 2;
        }
        setController(controlType);
    }
}