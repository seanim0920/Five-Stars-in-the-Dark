using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickTurnFailure : ObstacleFailure
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        failureDialogues = Resources.LoadAll<AudioClip>(SceneManager.GetActiveScene().name + "/Failure/QuickTurn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void playFailure(Vector3 point)
    {
        // Play failure dialogue sequentially
        if(numDialogue < failureDialogues.Length)
        {
            dialogueSource.clip = failureDialogues[numDialogue];
            numDialogue++;
        }
        base.playFailure(point);
    }
}
