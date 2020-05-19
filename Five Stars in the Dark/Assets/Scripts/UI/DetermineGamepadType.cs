using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetermineGamepadType : MonoBehaviour
{
    [SerializeField]
    private GameObject PS4;
    [SerializeField]
    private GameObject Xbox;
    public void DisplayType(bool isActive)
    {
        // Check if gamepad controls are enabled
        if(SettingsManager.toggles[2])
        {
            // GameObject.Find("GamepadControls").SetActive(isActive);
            if(Gamepad.current.name.Contains("DualShock"))
            {
                // Display PS4 controller
                PS4.SetActive(isActive);
                Xbox.SetActive(!isActive);
            }
            else
            {
                // Display Xbox controller
                Xbox.SetActive(isActive);
                PS4.SetActive(!isActive);
            }
        }
    }
}
