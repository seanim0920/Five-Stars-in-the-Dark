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
        if(Gamepad.current == null)
        {
            // Toggle Keyboard Controls
            keyboard.isOn = true;
        }
        else
        {
            // Toggle Gamepad Controls
            gamepad.isOn = true;
        }

        warning = "Controller not connected!";
    }

    public void checkWheel()
    {
    }

    public void checkGamepad()
    {
        if(Gamepad.current == null)
        {
            Debug.Log(warning);
            keyboard.isOn = true;
        }
    }
}
