﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour
{
    private PlayerControls controlFunctions;
    private float accelAmount = 0;
    private float breakAmount = 0;
    private float strafeAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        controlFunctions = GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up"))
        {
            controlFunctions.speedUp(accelAmount);
        }
        else if (Input.GetKey("down"))
        {
            controlFunctions.slowDown(breakAmount);
            strafeAmount *= 0.92f;
        }
        else
        {
            controlFunctions.returnToNeutralSpeed();
        }

        strafeAmount *= 0.97f;
        accelAmount *= 0.97f;
        breakAmount *= 0.97f;
        controlFunctions.strafe(strafeAmount);

        if (Input.GetKey("up") && accelAmount < 0.98f)
        {
            accelAmount += 0.02f;
        }
        if (Input.GetKey("down") && breakAmount < 0.98f)
        {
            breakAmount += 0.02f;
        }
        if (Input.GetKey("left") && strafeAmount > -0.98f)
        {
            strafeAmount -= 0.02f;
        }
        if (Input.GetKey("right") && strafeAmount < 0.98f)
        {
            strafeAmount += 0.02f;
        }
    }
}