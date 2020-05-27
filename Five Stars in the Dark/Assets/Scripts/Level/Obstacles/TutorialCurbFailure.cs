using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCurbFailure : ObstacleFailure
{
    [SerializeField]
    private GameObject curbTableaux;
    private static int numCurbCrashes = 0;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        failureDialogues = Resources.LoadAll<AudioClip>(SceneManager.GetActiveScene().name + "/Failure/ScriptedCurb");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void playFailure(Collision2D col)
    {
        if(numCurbCrashes - 1 < failureDialogues.Length)
        {
            Debug.Log("error made playing numCurbCrashes - 1: " + (numCurbCrashes - 1));
            StartCoroutine(PlayError.PauseDialogueCoroutine(failureDialogues[numCurbCrashes - 1]));
        }
        else
        {
            failureDialogues = Resources.LoadAll<AudioClip>(SceneManager.GetActiveScene().name + "/Failure/GenericCrash");

            System.Random rand = new System.Random();
            numDialogue = rand.Next(0, failureDialogues.Length);
            Debug.Log("error made playing numDialogue: " + numDialogue);
            StartCoroutine(PlayError.PauseDialogueCoroutine(failureDialogues[numDialogue]));
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            numCurbCrashes++;
            if(numCurbCrashes == 1)
            {
                var tableaux = Instantiate(curbTableaux);
                tableaux.GetComponent<DisplayStrafeTableaux>().tableauxNum = 2;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player") && numCurbCrashes <= 2)
        {
            playFailure(col);
        }
    }
}
