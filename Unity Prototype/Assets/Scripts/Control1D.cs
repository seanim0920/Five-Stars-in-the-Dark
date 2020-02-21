using System.Collections;
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
    private Vector3 movementDirection;
    private int invalidDirection = 0;
    void Start()
    {
        engineSound = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody2D>();
        bump = Resources.Load<AudioClip>("Audio/bumpend");
        movementDirection = transform.up;
    }
    void Update()
    {
        // Discrete turn l/r 
        transform.position += movementDirection * movementSpeed;
        //print(movementSpeed);
    }

    public void returnToNeutralSpeed()
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

    public void slowDown()
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
    public void speedUp()
    {
        if (movementSpeed < maxSpeed)
        {
            print("IT'S GOING");
            movementSpeed += acceleration;
            if (movementSpeed < neutralSpeed)
                engineSound.pitch += acceleration / neutralSpeed;
            else
                engineSound.pitch += 0.005f;
        }
    }
    public void blockDirection(int direction)
    {
        invalidDirection = direction;
    }
    public void strafe(float amount) //amount varies between -1 (steering wheel to the left) and 1 (steering wheel to the right)
    {
        //float objectSize = transform.localScale.magnitude / 2f;
        //0.1 is how close it can get to the curb before autostop
        if (invalidDirection / amount > 0) return;
        transform.position += amount * 2 * movementSpeed * transform.right;
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
