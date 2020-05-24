using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a singleton! Access the signleton with ScoreStorage.Instance
//This will give you the singular existing object which has this bad boy!
public class ScoreStorage : Singleton<ScoreStorage>
{
    //how far in the level the player progressed. If level passed, should always be 100
    int progress = 0;
    //number of errors
    [SerializeField]
    float errors = 0;
    //time taken to finish the level
    int time = 0;
    //literal score, from 0 to 100
    int points = 0;
    //fastest time the player could have achieved
    int par = 0;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.Find(gameObject.name))
        {
            if (Instance != this)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void resetScore()
    {
        print("resetting score");
        progress = 0;
        errors = 0;
        time = 0;
        points = 0;
        par = 0;
    }

    //This method attempts to set all the scores at once, based on where I (Thomas) think they are.
    public void setScoreAll()
    {
        //if the following scripts aren't in the scene, this method will fail:
        //ConstructLEvelFromMarkers (attached to LevelConstructor)
        //CheckErrors (attached to ErrorText, which is on the prefab Camera)
        //CountdownTimer (attached to TimerText, which is on the prefab Camera)
        progress = (int)ConstructLevelFromMarkers.getProgress();
        print("progress is " + progress);
        errors = TrackErrors.getErrors();
        time = (int)(600.0f - CountdownTimer.getCurrentTime()) * 100;
        //Par is not set here, becuase its data is not saved.
        //Every error subtracts 8 points, and every 30 seconds over the fastest possible time subtracts 5 points (up to 120 seconds).
        //this line was removed before the merge for some reason
        points = (int)(100 - (errors * 8) - ((time-par)/600));   //We divide be 600: 6 to split the time into 5 parts of 30 seconds and 100 to round out the miliseconds    
    }

    //These allow scripts to access the scores
    public int getScoreProgress()
    {
        return progress;
    }

    public float getScoreErrors()
    {
        return errors;
    }

    public int getScoreTime()
    {
        return time;
    }

    public int getScorePar()
    {
        return par;
    }

    public int getScorePoints()
    {
        return points;
    }

    //returns the time as a string formatted (XX:XX)
    public string getScoreTimeFormatted()
    {
        return time / 100 + ":" + time % 100;// time.toString("00:00");
    }

    //These allow scripts to manually set the scores
    public void setScoreProgress(int x)
    {
        progress = x;
    }

    public void setScoreErrors(float x)
    {
        errors = x;
    }

    public void setScoreTime(int x)
    {
        time = x;
    }
    
    public void setScorePar(int x)
    {
        par = x;
    }

    public void setScorePoints(int x)
    {
        points = x;
    }
}
