// NEED TO MAKE THIS FILE RESPOND TO EXPLICIT LEFT/RIGHT TURN INPUT
// AND NOT HARD-CODE KEYBOARD INPUT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTurn : MonoBehaviour
{
    //public int direction;
    private AudioSource turnSound;
    public KeyboardControl keyboardCtrl;
    public PlayerControls playerCtrl;
    public bool mustTurnLeft;
    private string turnDirection;
    // Start is called before the first frame update
    void Start()
    {
        if(mustTurnLeft)
        {
            turnDirection = "left";
        }
        else
        {
            turnDirection = "right";
        }

        turnSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // if (transform.parent.tag == "Right")
        // {
        //     direction = Resources.Load<AudioClip>("Audio/gps_right");
        // }
        // else
        // {
        //     direction = Resources.Load<AudioClip>("Audio/gps_left");
        // }
        // if (other.gameObject.tag == "Player")
        // {
        //     StartCoroutine(PlayWarnings());
        // }

        // Begin Quick Turn sequence
        if(other.transform.tag == "Player")
        {
            // Debug.Log("Turn " + turnDirection + "!"); // We potentially want to play a quick turn warning audio clip
            StartCoroutine(QTurn());
        }
    }
    IEnumerator QTurn()
    {
        float startTime = Time.time;
        // If player was strafing in wrong direction/holding wrong button in the first place
        while(!Input.GetKey(turnDirection) && Time.time - startTime < 2f)
        {
            // Wait for player to turn in correct direction (Make sure player is not cheating by somehow performing both inputs)
            yield return null;
        }

        startTime = Time.time;
        // If turning in correct direction
        if(Input.GetKey(turnDirection))
        {
            keyboardCtrl.enabled = false;
            playerCtrl.enabled = false;
            // Wait for a second and make sure player is holding correct direction the whole time
            while(Time.time - startTime < 1.5f)
            {
                yield return null;
            }
            // Play turnsound
            turnSound.Play();
            // return with no errors
            keyboardCtrl.enabled = true;
            playerCtrl.enabled = true;
            yield break;
        }
        // else (turned in wrong direction)
        else
        {
            // return with score decremented
            // Debug.Log("Decrement Score"); // We potentially want to play an error audio clip
            CheckErrors.IncrementErrorsAndUpdateDisplay();
            yield break;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // ...
    }

    void OnTriggerExit2D(Collider2D other)
    {
    }
}