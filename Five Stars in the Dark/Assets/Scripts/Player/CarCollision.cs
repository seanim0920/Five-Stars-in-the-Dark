using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarCollision : MonoBehaviour
{
    private Rigidbody2D body;
    private PlayerControls controlFunctions;
    private SteeringWheelInput wheelFunctions;

    private GameObject hitSoundObject;
    public GameObject situationalDialogues;
    //collision sound
    public AudioSource charOnCar;
    string[] obstacleTags = { "Car", "Curb", "Guardrail", "Pedestrian", "Stop", "Target" };

    // Start is called before the first frame update
    void Start()
    {
        controlFunctions = GetComponent<PlayerControls>();
        wheelFunctions = GetComponent<SteeringWheelInput>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator disableNPCMomentarily(GameObject NPC, float speed)
    {
        NPCMovement movement = NPC.GetComponent<NPCMovement>();
        Rigidbody2D NPCbody = NPC.GetComponent<Rigidbody2D>();
        movement.movementSpeed = 0;
        movement.enabled = false;
        NPCbody.AddForce((NPC.transform.position - transform.position).normalized * speed * 70, ForceMode2D.Impulse);
        if (speed > 0.5f)
        {
            if (speed > 0.8f)
            {
                NPC.transform.Find("SkidSfx").GetComponent<AudioSource>().Play();
            }
            movement.neutralSpeed = 0;
        }
        while (NPCbody.velocity.magnitude > 0.1f)
        {
            yield return new WaitForFixedUpdate();
        }
        NPCbody.velocity *= 0;
        if (speed > 0.5f)
        {
            NPC.transform.Find("AlarmSfx").GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(1);
            if (speed > 0.8f)
            {
                NPC.transform.Find("SmokeSfx").GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(1);
            }
            NPC.transform.Find("OpenDoorSfx").GetComponent<AudioSource>().Play();
        } else
        {
            NPC.transform.Find("HonkSfx").GetComponent<AudioSource>().Play();
            if (movement.neutralSpeed > 0)
                movement.enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (System.Array.IndexOf(obstacleTags, col.gameObject.tag) != -1)
        {
            //factor speed in, faster speed means bigger error
            TrackErrors.IncrementErrors();
        }

        hitSoundObject = col.gameObject;
        hitSoundObject.GetComponent<AudioSource>().Play();

        if (col.gameObject.CompareTag("Car") || col.gameObject.CompareTag("Target"))
        {
            wheelFunctions.PlayFrontCollisionForce();

            NPCMovement movementScript = col.gameObject.GetComponent<NPCMovement>();
            float speedDifference = Mathf.Abs(movementScript.movementSpeed - controlFunctions.movementSpeed);

            body.bodyType = RigidbodyType2D.Dynamic;
            body.AddForce((transform.position - col.gameObject.transform.position).normalized * speedDifference * 40, ForceMode2D.Impulse);
            StartCoroutine(controlFunctions.impact(body.velocity));
            if (!col.gameObject.CompareTag("Target"))
                StartCoroutine(disableNPCMomentarily(col.gameObject, speedDifference));

            if (hitSoundObject.GetComponent<ObstacleFailure>())
                hitSoundObject.GetComponent<ObstacleFailure>().playFailure(Camera.main.transform.position);
        }
        if (col.gameObject.CompareTag("Pedestrian") || col.gameObject.CompareTag("Stop"))
        {
            hitSoundObject.GetComponent<AudioSource>().Play();
        }
        if (col.gameObject.CompareTag("Guardrail"))
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
        int x = 4; // Random.Range(-2, 1) + (GetNumericValue(SceneManagment.Scene.name[6]) * 3);
        AudioClip passengerHurt = Resources.Load<AudioClip>("Audio/dialogue/hurt" + x);

        print("hitting a zone?" + (!col.gameObject.CompareTag("Zone")));
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Car"))
        {
            //charOnCar.Play();
        }
        if (col.gameObject.CompareTag("Guardrail"))
        {
            controlFunctions.blockDirection(0);
            col.gameObject.GetComponent<AudioSource>().Stop();
        }
        if (col.gameObject.CompareTag("Stop"))
        {
            TrackErrors.IncrementErrors();
        }

    }
}