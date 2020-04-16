using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a singleton! Access the signleton with ScoreStorage.Instance
//This will give you the singular existing object which has this bad boy!
public class ScoreStorage : Singleton<ScoreStorage>
{
    int progress;
    int errors;
    int time;

    /*public void pullScores()
    {
        //this will pull from publicly set methods to try and grab every score at once
    }*/

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
