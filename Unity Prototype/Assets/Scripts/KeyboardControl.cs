﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour
{
    private Control1D controlFunctions;
    private float strafeAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        controlFunctions = GetComponent<Control1D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey("up"))
        {
            controlFunctions.speedUp();
        }
        else if (Input.GetKey("down"))
        {
            controlFunctions.slowDown();
            strafeAmount *= 0.92f;
        }
        else
        {
            controlFunctions.returnToNeutralSpeed();
        }

        controlFunctions.strafe(strafeAmount);

        if (Input.GetKey("left") && strafeAmount > -0.98f)
        {
            strafeAmount -= 0.02f;
        }
        if (Input.GetKey("right") && strafeAmount < 0.98f)
        {
            strafeAmount += 0.02f;
        }
        */
    }
}