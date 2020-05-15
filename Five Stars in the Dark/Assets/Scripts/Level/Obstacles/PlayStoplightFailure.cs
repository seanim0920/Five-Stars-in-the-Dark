using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayStoplightFailure : ObstacleFailure
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        failureDialogues = Resources.LoadAll<AudioClip>(SceneManager.GetActiveScene().name + "/Failure/Stoplight");
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
