using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCarFailure : ObstacleFailure
{
    private static int numCrashes = 0;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        failureDialogues = Resources.LoadAll<AudioClip>(SceneManager.GetActiveScene().name + "/Failure/CrashClips/Scripted");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void playFailure(Vector3 point)
    {
        // Play scripted dialogue for crashes 1, 2, and last
        if(numCrashes < 2 || numCrashes > 7)
        {
            selectScriptedClips();
        }
        else
        {
            selectGenericClips();
        }
        base.playFailure(point);
        // Play generic crash dialogue for all other crashes
    }

    private void selectScriptedClips()
    {
        failureDialogues = Resources.LoadAll<AudioClip>(SceneManager.GetActiveScene().name + "/Failure/CrashClips/Scripted");
        if(numCrashes < failureDialogues.Length - 1)
        {
            dialogueSource.clip = failureDialogues[numCrashes];
            numCrashes++;
        }
        else
        {
            dialogueSource.clip = failureDialogues[failureDialogues.Length - 1];
        }
    }

    private void selectGenericClips()
    {
        failureDialogues = Resources.LoadAll<AudioClip>(SceneManager.GetActiveScene().name + "/Failure/CrashClips/Generic");

        System.Random rand = new System.Random();
        numCrashes = rand.Next(0, failureDialogues.Length);
        Debug.Log("error made playing numCrashes: " + numCrashes);
        StartCoroutine(PlayError.PauseDialogueCoroutine(failureDialogues[numCrashes]));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            numCrashes++;
        }
    }
}
