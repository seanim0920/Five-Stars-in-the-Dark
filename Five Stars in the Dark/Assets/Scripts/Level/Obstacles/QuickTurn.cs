// NEED TO MAKE THIS FILE RESPOND TO EXPLICIT LEFT/RIGHT TURN INPUT
// AND NOT HARD-CODE KEYBOARD INPUT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickTurn : MonoBehaviour
{
    [SerializeField] private PS4Controls gamepad;
    //public int direction;
    private AudioSource turnSound;
    public KeyboardControl keyboardCtrl;
    public GamepadControl gamepadCtrl;
    public PlayerControls playerCtrl;
    public bool mustTurnLeft;
    private string turnDirection;

    void Awake()
    {
        gamepad = new PS4Controls();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(mustTurnLeft)
        {
            turnDirection = "Left";
        }
        else
        {
            turnDirection = "Right";
        }

        turnSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Begin Quick Turn sequence
        if(other.transform.tag == "Player")
        {
            keyboardCtrl = other.gameObject.GetComponent<KeyboardControl>();
            playerCtrl = other.gameObject.GetComponent<PlayerControls>();
            other.GetComponentInParent<GamepadControl>().gamepad.Gameplay.Disable();
            // other.GetComponentInParent<GamepadControl>().gamepad.QuickTurns.Enable();
            other.GetComponentInParent<GamepadControl>().gamepad.QuickTurns.Get().FindAction("Turn " + turnDirection).Enable();
            Debug.Log("Turn " + turnDirection + "!"); // We potentially want to play a quick turn warning audio clip
            StartCoroutine(QTurn(other.GetComponentInParent<GamepadControl>().gamepad));
        }
    }
    IEnumerator QTurn(PS4Controls gp)
    {
        float startTime = Time.time;
        // If player was strafing in wrong direction/holding wrong button in the first place
        // turnValue = gp.QuickTurns.Get().FindAction("Turn" + turnDirection).ReadValue<float>();
        // Debug.Log(gp.QuickTurns.Get().FindAction("Turn " + turnDirection).ReadValue<float>());

        keyboardCtrl.enabled = false;
        playerCtrl.enabled = false;
        while((!Input.GetKey(turnDirection.ToLower()) ||
               gp.QuickTurns.Get().FindAction("Turn " + turnDirection).ReadValue<float>() <= 0) &&
               Time.time - startTime < 2f)
        {
            // Debug.Log(gp.QuickTurns.Get().FindAction("Turn " + turnDirection).ReadValue<float>());
            // Wait for player to turn in correct direction (Make sure player is not cheating by somehow performing both inputs)
            yield return null;
        }

        startTime = Time.time;
        // If turning in correct direction
        if(Input.GetKey(turnDirection.ToLower()))
        {
            // Wait for a second and make sure player is holding correct direction the whole time
            while(Time.time - startTime < 1.0f)
            {
                yield return null;
            }
            // Play turnsound
            turnSound.Play();
            // return with no errors
            keyboardCtrl.enabled = true;
        }
        else if(gp.QuickTurns.Get().FindAction("Turn " + turnDirection).ReadValue<float>() > 0)
        {
            // Play turnsound
            turnSound.Play();
            gp.QuickTurns.Get().FindAction("Turn " + turnDirection).Disable();
            gp.Gameplay.Enable();
            // return with no errors
        }
        // else (turned in wrong direction)
        else
        {
            // return with score decremented
            Debug.Log("Decrement Score"); // We potentially want to play an error audio clip
            TrackErrors.IncrementErrors();
            gp.QuickTurns.Get().FindAction("Turn " + turnDirection).Disable();

            /////////////////////////////////////////////////////////////////////////////////////
            //                                                                                 //
            //                                                                                 //
            //  5/13/20                                                                        //
            //  - Just realized that enabling both of these when failing a quick turn can      //
            //    cause problems                                                               //
            //                                                                  - Charles      //
            //                                                                                 //
            /////////////////////////////////////////////////////////////////////////////////////
            gp.Gameplay.Enable();
            keyboardCtrl.enabled = true;
        }

        playerCtrl.enabled = true;
        Destroy(gameObject, turnSound.clip.length);
        yield break;
    }
}