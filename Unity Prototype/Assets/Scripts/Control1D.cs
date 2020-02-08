﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control1D : MonoBehaviour
{
    private float movementSpeed = 0f;
    private float maxSpeed = 0.1f;
    private float neutralSpeed = 0.05f;
    private float acceleration = 0.001f;
    private AudioSource engineSound;
    private Rigidbody2D body;
    private AudioClip bump;
    void Start()
    {
        engineSound = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody2D>();
        bump = Resources.Load<AudioClip>("Audio/bumpend");
    }
    void Update()
    {
        // Discrete turn l/r 
        transform.position += transform.up * movementSpeed;
        //print(movementSpeed);

        //print(neutralSpeed - movementSpeed);
        if ((Input.GetKey("up")))
        {
            speedUp();
        }
        else if (Input.GetKey("down"))
        {
            slowDown();
        }
        else
        {
            if (neutralSpeed > movementSpeed)
            {
                speedUp();
            }
            else if (movementSpeed > neutralSpeed)
            {
                slowDown();
            }
            else
            {
                movementSpeed = neutralSpeed;
                engineSound.pitch = 1;
            }
        }

        float viewxpos = Camera.main.WorldToViewportPoint(transform.position).x;
        if (Input.GetKey("left"))
        {
            transform.position += Mathf.Min(movementSpeed, 0.1f) * new Vector3(-0.5f, 0, 0);
            if (transform.position.x < 3.5f)
            {
                transform.position = new Vector3(3.5f, transform.position.y, transform.position.z);
            }
        }
        if (Input.GetKey("right"))
        {
            transform.position += Mathf.Min(movementSpeed, 0.1f) * new Vector3(0.5f, 0, 0);
            if (transform.position.x > 8.5f)
            {
                transform.position = new Vector3(8.5f, transform.position.y, transform.position.z);
            }
        }
    }

    void slowDown()
    {
        if (movementSpeed < neutralSpeed)
        {
            movementSpeed *= 0.93f;
            engineSound.pitch *= 0.93f;
        }
        else
        {
            movementSpeed -= acceleration;
            engineSound.pitch -= 0.005f;
        }
    }
    void speedUp()
    {
        if (movementSpeed < maxSpeed)
        {
            movementSpeed += acceleration;
            if (movementSpeed < neutralSpeed)
                engineSound.pitch += acceleration / neutralSpeed;
            else
                engineSound.pitch += 0.005f;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            movementSpeed *= 0.1f;
            engineSound.pitch *= 0.1f;
        }
    }
}
