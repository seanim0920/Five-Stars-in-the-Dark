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
            other.GetComponentInParent<GamepadControl>().gamepad.Gameplay.Disable();
            other.GetComponentInParent<GamepadControl>().gamepad.QuickTurns.Enable();
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

        while((!Input.GetKey(turnDirection.ToLower()) ||
               gp.QuickTurns.Get().FindAction("Turn " + turnDirection).ReadValue<float>() > 0) &&
               Time.time - startTime < 2f)
        {
            Debug.Log(gp.QuickTurns.Get().FindAction("Turn " + turnDirection).ReadValue<float>());
            // Wait for player to turn in correct direction (Make sure player is not cheating by somehow performing both inputs)
            yield return null;
        }

        startTime = Time.time;
        // If turning in correct direction
        if(Input.GetKey(turnDirection.ToLower()) ||
           gp.QuickTurns.Get().FindAction("Turn " + turnDirection).ReadValue<float>() > 0)
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
            gp.QuickTurns.Disable();
            gp.Gameplay.Enable();
            yield break;
        }
        // else (turned in wrong direction)
        else
        {
            // return with score decremented
            // Debug.Log("Decrement Score"); // We potentially want to play an error audio clip
            CheckErrors.IncrementErrorsAndUpdateDisplay();
            gp.QuickTurns.Disable();
            gp.Gameplay.Enable();
            yield break;
        }
    }
}