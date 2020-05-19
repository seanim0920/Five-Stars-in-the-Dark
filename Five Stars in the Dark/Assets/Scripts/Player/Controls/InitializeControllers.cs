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
            Debug.Log(Gamepad.current.name);
        }
        else
        {
            // Toggle Keyboard Controls
            keyboard.isOn = true;
        }

        warning = "Controller not connected!";

        InputSystem.onDeviceChange +=
            (device, change) =>
            {
                switch (change)
                {
                    // case InputDeviceChange.Added:
                    //     Debug.Log("Device added: " + device);
                    //     break;
                    case InputDeviceChange.Removed:
                        // Debug.Log("Device removed: " + device);
                        checkGamepad();
                        break;
                    // case InputDeviceChange.ConfigurationChanged:
                    //     Debug.Log("Device configuration changed: " + device);
                    //     break;
                }
            };
    }

    public void checkWheel()
    {
        if(!(SteeringWheelInput.checkConnected()))
        {
            // Debug.Log(warning);
            keyboard.isOn = true;
            steeringWheel.isOn = !keyboard.isOn;
            SettingsManager.toggles[1] = keyboard.isOn;
        }
    }

    public void checkGamepad()
    {
        if(Gamepad.current == null)
        {
            // Debug.Log(warning);
            keyboard.isOn = true;
            gamepad.isOn = !keyboard.isOn;
            SettingsManager.toggles[1] = keyboard.isOn;
        }
    }
}
