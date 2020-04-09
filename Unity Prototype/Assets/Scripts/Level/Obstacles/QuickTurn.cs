// NEED TO MAKE THIS FILE RESPOND TO EXPLICIT LEFT/RIGHT TURN INPUT
// AND NOT HARD-CODE KEYBOARD INPUT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTurn : MonoBehaviour
{
    //public int direction;
    public AudioSource turnSound;
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
        StartCoroutine(QTurn());
    }
    IEnumerator QTurn()
    {
        // If player was strafing in wrong direction/holding wrong button in the first place
        if(!Input.GetKey(turnDirection))
        {
            // Wait for player to turn in correct direction (Make sure player is not cheating by somehow performing both inputs)
            yield return new WaitForSeconds(2f);
        }

        float startTime = Time.time;
        // If turning in correct direction
        if(Input.GetKey(turnDirection))
        {
            // Wait for a second and make sure player is holding correct direction the whole time
            while(Time.time - startTime < 1f)
            {
                yield return null;
            }
            // Play turnsound
            turnSound.Play();
            // return with no errors
            yield break;;
        }
        // else (turned in wrong direction)
        else
        {
            // return with score decremented
            Debug.Log("Decrement Score");
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