using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InitializeControllers : MonoBehaviour
{
    [SerializeField] private Toggle steeringWheel;
    [SerializeField] private Toggle keyboard;
    [SerializeField] private Toggle gamepad;
    private string warning;
    // Start is called before the first frame update
    void Start()
    {
        SettingsManager.setToggles();
        if (SettingsManager.toggles[0])
        {
            // Toggle Steering Wheel Controls
            steeringWheel.isOn = true;
        }
        else if(SettingsManager.toggles[2])
        {
            // Toggle Gamepad Controls
            gamepad.isOn = true;
        }
        else
        {
            // Toggle Keyboard Controls
            keyboard.isOn = true;
        }

        warning = "Controller not connected!";
    }

    public void checkWheel()
    {
        if(!(SteeringWheelInput.checkConnected()))
        {
            // Debug.Log(warning);
            keyboard.isOn = true;
            SettingsManager.toggles[1] = keyboard.isOn;
        }
    }

    public void checkGamepad()
    {
        if(Gamepad.current == null)
        {
            // Debug.Log(warning);
            keyboard.isOn = true;
            SettingsManager.toggles[1] = keyboard.isOn;
        }
    }
}
