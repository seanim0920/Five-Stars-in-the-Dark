using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericObstacleFailure : ObstacleFailure
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        failureDialogues = Resources.LoadAll<AudioClip>(SceneManager.GetActiveScene().name + "/Failure/GenericCrash");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void playFailure(Vector3 point)
    {
        // Play random failure dialogue
        System.Random rand = new System.Random();
        numDialogue = rand.Next(0, failureDialogues.Length);
        Debug.Log("numDialogue: " + numDialogue);
        PlayError.PauseDialogue(failureDialogues[numDialogue]);
        //base.playFailure(point);
    }
}