using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarCollision : MonoBehaviour
{
    private Rigidbody2D body;
    private PlayerControls controlFunctions;
    private SteeringWheelControl wheelFunctions;

    public ConstructLevelFromMarkers cutsceneScript;
    public GameObject hitSoundObject;
    public GameObject situationalDialogues;
    //collision sound
    public AudioSource charOnCar;
    string[] obstacleTags = { "Car", "Curb", "Guardrail", "Pedestrian", "Stop" };

    // Start is called before the first frame update
    void Start()
    {
        controlFunctions = GetComponent<PlayerControls>();
        wheelFunctions = GetComponent<SteeringWheelControl>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (System.Array.IndexOf(obstacleTags, col.gameObject.tag) != -1)
        {
            if (!cutsceneScript.isSpeaking)
            {
                int randomChildIdx = Random.Range(0, situationalDialogues.transform.childCount);
                Transform randomChild = situationalDialogues.transform.GetChild(randomChildIdx);
                randomChild.GetComponent<AudioSource>().Play();
            }
            CheckErrors.IncrementErrorsAndUpdateDisplay();
        }

        hitSoundObject = col.gameObject;

        if (col.gameObject.tag == "Car")
        {
            hitSoundObject.transform.position = col.gameObject.transform.position;
            hitSoundObject.GetComponent<AudioSource>().Play();
            wheelFunctions.PlayFrontCollisionForce();
            controlFunctions.movementSpeed *= 0.1f;
            //setRadioTempo(getRadioTempo() * 0.1f);
        }
        if (col.gameObject.tag == "Pedestrian" || col.gameObject.tag == "Stop")
        {
            hitSoundObject.GetComponent<AudioSource>().Play();
        }
        if (col.gameObject.tag == "Guardrail")
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f);
           if (hit.collider != null && hit.collider.gameObject.tag == col.gameObject.tag)
           {
                controlFunctions.blockDirection(1);
           } else
           {
                controlFunctions.blockDirection(-1);
           }

            //this statement pans the audio depending on which side the guardrail is on
            hitSoundObject.GetComponent<AudioSource>().panStereo = this.gameObject.transform.position.x > hitSoundObject.transform.position.x ? -1 : 1;
            //this statement plays the audio
            hitSoundObject.GetComponent<AudioSource>().Play();
        }

        //these pull a random hurtsound to play
        int x = 1; // Random.Range(-2, 1) + (GetNumericValue(SceneManagment.Scene.name[6]) * 3);
        AudioClip passengerHurt = Resources.Load<AudioClip>("Audio/dialogue/" + SceneManager.GetActiveScene().name + "/hurt" + x);
        
        //if the passenger is speaking...
        if (cutsceneScript.checkIfSpeaking() && cutsceneScript.levelDialogue.isPlaying)
        {
            //play the hurtsound and wait 3 seconds
            cutsceneScript.isSpeaking = false;
            StartCoroutine(HitsoundWait(passengerHurt, 3));
            cutsceneScript.isSpeaking = true;
        }
        //if not...
        else
        {
            //just play the hurtsound
            AudioSource.PlayClipAtPoint(passengerHurt, new Vector3(0, 0, 0));
        }

    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            //charOnCar.Play();
        }
        if (col.gameObject.tag == "Guardrail")
        {
            controlFunctions.blockDirection(0);
            col.gameObject.GetComponent<AudioSource>().Stop();
        }
        if (col.gameObject.tag == "Stop")
        {
            CheckErrors.IncrementErrorsAndUpdateDisplay();
        }

    }

    IEnumerator HitsoundWait(AudioClip passengerHurt, int x)
    {
        Debug.Log("Pausing Dialogue");
        //Stop the level dialogue
        cutsceneScript.levelDialogue.Stop();
        //play a random hurtsound
        AudioSource.PlayClipAtPoint(passengerHurt, /*new Vector3(0, 0, 0)*/this.gameObject.transform.position);
        //rewind to when this dialogue section started
        cutsceneScript.levelDialogue.time = cutsceneScript.currentDialogueStartTime;
        //wait for... idk 3 seconds?
        yield return new WaitForSeconds(x);
        //resume dialogue
        cutsceneScript.levelDialogue.Play();
        Debug.Log("Resuming Dialogue");
    }

}