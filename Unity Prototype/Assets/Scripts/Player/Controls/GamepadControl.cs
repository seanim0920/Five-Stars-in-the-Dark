using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadControl : MonoBehaviour
{
    public PS4Controls gamepad;
    [SerializeField] private PlayerInput input;
    [SerializeField] private PlayerControls controlFunctions;
    private KeyboardControl keyboardScript;
    private float accelAmt = 0;
    private bool isAccelerating;
    private float brakeAmt = 0;
    private bool isBraking;
    private float strafeAmt = 0;
    private bool isStrafing;

    void Awake()
    {
        gamepad = new PS4Controls();
    }
    // Start is called before the first frame update
    void Start()
    {
        keyboardScript = GetComponent<KeyboardControl>();
        isAccelerating = false;
        isBraking = false;
        isStrafing = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(keyboardScript.enabled)
        {
            keyboardScript.enabled = false;
        }

        if(isBraking)
        {
            controlFunctions.slowDown(brakeAmt);
        }
        else if(isAccelerating)
        {
            controlFunctions.speedUp(accelAmt);
        }
        else
        {
            controlFunctions.returnToNeutralSpeed();
        }

        if(isStrafing)
        {
            controlFunctions.strafe(strafeAmt);
        }

        Debug.Log("Gameplay:" + gamepad.Gameplay.enabled);
        Debug.Log("QuickTurns: " + gamepad.QuickTurns.enabled);
    }

    private void OnEnable()
    {
        // if(!gamepad.QuickTurns.enabled)
        // {
            gamepad.Gameplay.Accelerate.performed += HandleAccelerate;
            gamepad.Gameplay.Accelerate.canceled += CancelAccelerate;
            gamepad.Gameplay.Accelerate.Enable();

            gamepad.Gameplay.Brake.performed += HandleBrake;
            gamepad.Gameplay.Brake.canceled += CancelBrake;
            gamepad.Gameplay.Brake.Enable();

            gamepad.Gameplay.Strafe.performed += HandleStrafe;
            gamepad.Gameplay.Strafe.canceled += CancelStrafe;
            gamepad.Gameplay.Strafe.Enable();

        //     gamepad.QuickTurns.Disable();
        // }
        // else
        // {
        //     Debug.Log("Enabling Quick Turns");
            gamepad.QuickTurns.TurnLeft.performed += Turning;
            gamepad.QuickTurns.TurnLeft.canceled += CancelTurning;
            // gamepad.QuickTurns.TurnLeft.Enable();

            gamepad.QuickTurns.TurnRight.performed += Turning;
            gamepad.QuickTurns.TurnRight.canceled += CancelTurning;
            // gamepad.QuickTurns.TurnRight.Enable();
        // }
    }

    private void OnDisable()
    {
        gamepad.Gameplay.Accelerate.performed -= HandleAccelerate;
        gamepad.Gameplay.Brake.canceled -= CancelAccelerate;
        gamepad.Gameplay.Accelerate.Disable();
        
        gamepad.Gameplay.Brake.performed -= HandleBrake;
        gamepad.Gameplay.Brake.canceled -= CancelBrake;
        gamepad.Gameplay.Brake.Disable();

        gamepad.Gameplay.Strafe.performed -= HandleStrafe;
        gamepad.Gameplay.Strafe.canceled -= CancelStrafe;
        gamepad.Gameplay.Strafe.Disable();

        gamepad.QuickTurns.TurnLeft.performed -= Turning;
        gamepad.QuickTurns.TurnLeft.canceled -= CancelTurning;
        gamepad.QuickTurns.TurnLeft.Disable();

        gamepad.QuickTurns.TurnRight.performed -= Turning;
        gamepad.QuickTurns.TurnRight.canceled -= CancelTurning;
        gamepad.QuickTurns.TurnRight.Disable();
    }

    private void HandleAccelerate(InputAction.CallbackContext context)
    {
        accelAmt = context.ReadValue<float>();
        isAccelerating = accelAmt >= 0.1;
    }

    private void CancelAccelerate(InputAction.CallbackContext context)
    {
        isAccelerating = false;
    }

    private void HandleBrake(InputAction.CallbackContext context)
    {
        brakeAmt = context.ReadValue<float>() / 50;
        isBraking = brakeAmt > 0.1 / 50;
    }

    private void CancelBrake(InputAction.CallbackContext context)
    {
        isBraking = false;
    }

    private void HandleStrafe(InputAction.CallbackContext context)
    {
        Debug.Log("Strafing");
        strafeAmt = context.ReadValue<float>() / 2;
        isStrafing = Mathf.Abs(strafeAmt) > 0.1 / 50;
    }

    private void CancelStrafe(InputAction.CallbackContext context)
    {
        isStrafing = false;
    }

    private void Turning(InputAction.CallbackContext context)
    {
        Debug.Log("Turning");
        var value = context.ReadValue<float>();
        Debug.Log(value);
    }

    private void CancelTurning(InputAction.CallbackContext context)
    {
        Debug.Log("Cancel turning");
        var value = context.ReadValue<float>();
        Debug.Log(value);
        gamepad.Gameplay.Enable();
        gamepad.QuickTurns.Disable();
    }
}
