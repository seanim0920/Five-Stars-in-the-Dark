using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarCollision : MonoBehaviour
{
    private Rigidbody2D body;
    private PlayerControls controlFunctions;
    private SteeringWheelControl wheelFunctions;

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
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator disablePlayerControlMomentarily(float disabledTime)
    {
        controlFunctions.movementSpeed = 0;
        controlFunctions.enabled = false;
        yield return new WaitForSeconds(disabledTime);
        controlFunctions.enabled = true;
    }

    IEnumerator disableNPCMovementMomentarily(GameObject NPC, float disabledTime)
    {
        NPCMovement movement = NPC.GetComponent<NPCMovement>();
        movement.movementSpeed = 0;
        movement.enabled = false;
        yield return new WaitForSeconds(disabledTime);
        movement.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (System.Array.IndexOf(obstacleTags, col.gameObject.tag) != -1)
        {
            CheckErrors.IncrementErrorsAndUpdateDisplay();
        }

        hitSoundObject = col.gameObject;

        if (col.gameObject.tag == "Car")
        {
            hitSoundObject.transform.position = col.gameObject.transform.position;
            hitSoundObject.GetComponent<AudioSource>().Play();
            wheelFunctions.PlayFrontCollisionForce();
            if (col.gameObject.GetComponent<NPCMovement>() != null)
            {
                col.gameObject.GetComponent<NPCMovement>().enabled = false;
            }
            if (col.gameObject.GetComponent<PoliceMovement>() != null)
            {
                col.gameObject.GetComponent<PoliceMovement>().enabled = false;
            }
            col.gameObject.GetComponent<Rigidbody2D>().AddForce((col.gameObject.transform.position - transform.position).normalized, ForceMode2D.Impulse);
            //should be adjusted to push them away from the car rather than just slowing them down
            controlFunctions.movementSpeed *= 0.1f;
            //setRadioTempo(getRadioTempo() * 0.1f);
        }
        if (col.gameObject.tag == "Pedestrian" || col.gameObject.tag == "Stop")
        {
            hitSoundObject.GetComponent<AudioSource>().Play();
        }
        if (col.gameObject.tag == "Guardrail")
        {
           if (col.gameObject.transform.position.x > transform.position.x)
           {
                print("blocked right");
                controlFunctions.blockDirection(1);
           } else
            {
                print("blocked left");
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

        print("hitting a zone?" + (col.gameObject.tag != "Zone"));
        if (col.gameObject.tag != "Zone")
        {
            //if the passenger is speaking...
            if (ConstructLevelFromMarkers.isSpeaking && ConstructLevelFromMarkers.levelDialogue.isPlaying)
            {
                //play the hurtsound and wait 3 seconds
                ConstructLevelFromMarkers.isSpeaking = false;
                StartCoroutine(HitsoundWait(passengerHurt, 2));
            }
            //if not...
            else
            {
                //just play the hurtsound
                AudioSource.PlayClipAtPoint(passengerHurt, new Vector3(0, 0, 0));
            }
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
        AudioSource dialogue = ConstructLevelFromMarkers.levelDialogue;
        //play a random hurtsound
        AudioSource.PlayClipAtPoint(passengerHurt, /*new Vector3(0, 0, 0)*/this.gameObject.transform.position);

        //find the last silent section of audio and rewind to it, or if not just rewind back a set amount of time
        float maxRewindTime = 2; //in seconds
        int maxRewindTimeInSamples = (int)((int)(dialogue.clip.samples / dialogue.clip.length) * maxRewindTime);
        float[] samples = new float[maxRewindTimeInSamples * dialogue.clip.channels]; //array to be filled with samples from the audioclip

        int currentTimePosition = dialogue.timeSamples - maxRewindTimeInSamples; //by default
        dialogue.clip.GetData(samples, currentTimePosition);

        //Stop the level dialogue (we aren't using pause, that's reserved for the pause menu)
        dialogue.Stop();

        int foundSilences = 0;
        for (int i = samples.Length; i-- > 0;)
        {
            if (Mathf.Abs(samples[i]) == 0f)
            {
                foundSilences++;
                if (foundSilences >= 2)
                {
                    print("found silences.");
                    currentTimePosition += (i / dialogue.clip.channels);
                    break;
                }
            }
            else
            {
                foundSilences = 0;
            }
        }

        //cutsceneScript.levelDialogue.time = cutsceneScript.currentDialogueStartTime;
        //wait for... idk 3 seconds?
        yield return new WaitForSeconds(passengerHurt.length + 1f);
        //resume dialogue
        dialogue.timeSamples = currentTimePosition;
        dialogue.Play();
        ConstructLevelFromMarkers.isSpeaking = true;
        Debug.Log("Resuming Dialogue");
    }

}