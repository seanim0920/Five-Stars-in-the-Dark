using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour
{
    private Control1D controlFunctions;
    // Start is called before the first frame update
    void Start()
    {
        controlFunctions = GetComponent<Control1D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up"))
        {
            controlFunctions.speedUp();
        }
        else if (Input.GetKey("down"))
        {
            controlFunctions.slowDown();
        }
        else
        {
            controlFunctions.returnToNeutralSpeed();
        }

        if (Input.GetKey("left"))
        {
            controlFunctions.strafe(-1);
        }
        if (Input.GetKey("right"))
        {
            controlFunctions.strafe(1);
        }
    }
}