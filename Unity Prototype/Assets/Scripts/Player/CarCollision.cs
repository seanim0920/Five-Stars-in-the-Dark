using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    string[] obstacleTags = { "Curb", "Guardrail", "Pedestrian", "Stop" };

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

        DialogueInterrupt();
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

    void DialogueInterrupt()
    {
        //pick a number in [1, 4) to determine hitsound
        int x = Random.Range(-2, 1) + (GetNumericValue(SceneManagment.Scene.name[6]) * 3);
        //the only differences between the hurt sounds (besides content) are their names so I'm just using that
        //If all levels are named 'Level i' and all hurt sounds are called 'hurt x', 'hurt x+1', and hurt x+2',
        //then this will pick a hurt number in the correct range for the level.
        
        //is there dialogue currently being spoken? If so...
        if (cutsceneScript.isSpeaking)
        {
            //Stop the level dialogue
            cutsceneScript.levelDialogue.Stop(); 
            //play a random hurtsound
            //The following line assumes the clip is played with 2D stereo (i.e. won't get quieter as the car moves)
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/" + SceneManagment.Scene.name + "/hurt " + x), new Vector3(0, 0, 0));
            //rewind to when this dialogue section started
            cutsceneScript.levelDialogue.time = cutsceneScript.currentDialogueStartTime;
            //wait for... idk 3 seconds?
            StartCoroutine(HitsoundWait(3));
            //resume dialogue
            cutsceneScript.levelDialogue.Play();
        }
        //if not...
        else
        {
            //just play a random hurtsound
            //The following line assumes the clip is played with 0 stereo (i.e. won't get quiter as the car moves)
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/" + SceneManagment.Scene.name + "/hurt" + x), new Vector3(0, 0, 0));
        }
    }

    //yes, I know it's a bit of a waste to have this IEnumerator here, but I can't declare it anonamously so oh well
    IEnumerator HitsoundWait(int x)
    {
        yield return new WaitForSeconds(x);
    }

}