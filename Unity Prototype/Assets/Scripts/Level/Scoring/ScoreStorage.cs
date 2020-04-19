using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a singleton! Access the signleton with ScoreStorage.Instance
//This will give you the singular existing object which has this bad boy!
public class ScoreStorage : Singleton<ScoreStorage>
{
    int progress = 0;
    int errors = 0;
    int time = 0;

    //DontDestroyOnLoad(this.gameObject);

    //This method attempts to set all the scores at once, based on where I (Thomas) think they are.
    public void setScoreAll()
    {
        //if the following scripts aren't in the scene, this method will fail:
        //ConstructLEvelFromMarkers (attached to LevelConstructor)
        //CheckErrors (attached to ErrorText, which is on the prefab Camera)
        //CountdownTimer (attached to TimerText, which is on the prefab Camera)
        ConstructLevelFromMarkers CLFM = GameObject.Find("/LevelConstructor").GetComponent<ConstructLevelFromMarkers>();
        progress = (int)(CLFM.currentDialogueStartTime * 100 / CLFM.endOfLevel);
        errors = GameObject.Find("/Main Camera/Canvas/ErrorText").GetComponent<CheckErrors>().getErrors();
        time = (int)(600.0f - GameObject.Find("/Main Camera/Canvas/TimerText").GetComponent<CountdownTimer>().getCurrentTime()) * 100;
    }

    //These allow scripts to access the scores
    public int getScoreProgress()
    {
        return progress;
    }

    public int getScoreErrors()
    {
        return errors;
    }

    public int getScoreTime()
    {
        return time;
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

    public void setScoreErrors(int x)
    {
        errors = x;
    }

    public void setScoreTime(int x)
    {
        time = x;
    }
}
