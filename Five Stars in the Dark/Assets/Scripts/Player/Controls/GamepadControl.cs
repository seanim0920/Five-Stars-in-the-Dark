using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadControl : MonoBehaviour
{
    public PS4Controls gamepad;
    [SerializeField] private PlayerControls controlFunctions;
    private KeyboardControl keyboardScript;
    private float accelAmt = 0f;
    private bool isAccelerating;
    private float brakeAmt = 0f;
    private bool isBraking;
    private float strafeVelocity = 0f;
    private float strafeAcceleration = 0f;
    private float strafeInitial = 0f;
    private float strafeFinal = 0f;
    private bool isStrafing;
    private bool gamepadConnected = false;
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
        if(gamepadConnected && keyboardScript.enabled)
        {
            keyboardScript.enabled = false;
        }

        if (isBraking || isAccelerating)
        {
            if (isBraking)
            {
                controlFunctions.slowDown(brakeAmt);
            }
            if (isAccelerating)
            {
                controlFunctions.speedUp(accelAmt);
            }
        }
        else
        {
            controlFunctions.returnToNeutralSpeed();
        }

        strafeAcceleration = (strafeFinal - strafeInitial) / 90f; // 90f is a magic number for the wheel rotation speed
        if(isStrafing)
        {
            // If velocity < 1/3 or velocity > 1/3, but acceleration is in opposite direction
            if(Mathf.Abs(strafeVelocity) < 0.33f)
            {
                strafeVelocity += strafeAcceleration;
                strafeInitial = Mathf.Lerp(strafeInitial, strafeFinal, 0.05f);
            }
            if(Mathf.Abs(strafeAcceleration + strafeVelocity) < 0.33f)
            {
                strafeVelocity += strafeAcceleration * 1.4f;
            }
            controlFunctions.strafe(strafeVelocity);
            if (!controlFunctions.enabled) strafeVelocity = 0;

        }
        else
        {
            strafeVelocity *= 0.97f;
            if (!controlFunctions.enabled) strafeVelocity = 0;
        }
        controlFunctions.strafe(strafeVelocity);
    }

    private void OnEnable()
    {
        gamepad.Gameplay.Accelerate.performed += HandleAccelerate;
        gamepad.Gameplay.Accelerate.canceled += CancelAccelerate;
        gamepad.Gameplay.Accelerate.Enable();

        gamepad.Gameplay.Brake.performed += HandleBrake;
        gamepad.Gameplay.Brake.canceled += CancelBrake;
        gamepad.Gameplay.Brake.Enable();

        gamepad.Gameplay.Strafe.started += StartStrafe;
        gamepad.Gameplay.Strafe.performed += HandleStrafe;
        gamepad.Gameplay.Strafe.canceled += CancelStrafe;
        gamepad.Gameplay.Strafe.Enable();
        
        gamepad.QuickTurns.TurnLeft.performed += Turning;
        gamepad.QuickTurns.TurnLeft.canceled += CancelTurning;

        gamepad.QuickTurns.TurnRight.performed += Turning;
        gamepad.QuickTurns.TurnRight.canceled += CancelTurning;
    }

    private void OnDisable()
    {
        gamepad.Gameplay.Accelerate.performed -= HandleAccelerate;
        gamepad.Gameplay.Brake.canceled -= CancelAccelerate;
        gamepad.Gameplay.Accelerate.Disable();
        
        gamepad.Gameplay.Strafe.started -= StartStrafe;
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
        isAccelerating = accelAmt >= 0.1f;
    }

    private void CancelAccelerate(InputAction.CallbackContext context)
    {
        isAccelerating = false;
    }

    private void HandleBrake(InputAction.CallbackContext context)
    {
        brakeAmt = context.ReadValue<float>() / 50f;
        isBraking = brakeAmt > 0.1f / 50f;
    }

    private void CancelBrake(InputAction.CallbackContext context)
    {
        isBraking = false;
    }

    private void StartStrafe(InputAction.CallbackContext context)
    {
        strafeInitial = 0f; // context.ReadValue<float>();
    }
    private void HandleStrafe(InputAction.CallbackContext context)
    {
        strafeFinal = context.ReadValue<float>();
        isStrafing = Mathf.Abs(strafeFinal) > 0.01f;
    }

    private void CancelStrafe(InputAction.CallbackContext context)
    {
        isStrafing = false;
        strafeInitial = 0f;
        strafeFinal = 0f;
    }

    private void Turning(InputAction.CallbackContext context)
    {
    }

    private void CancelTurning(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        gamepad.Gameplay.Enable();
        gamepad.QuickTurns.Disable();
    }
}
