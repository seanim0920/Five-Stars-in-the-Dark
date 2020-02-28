using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
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
    private float lastRecordedStrafe = 0;
    private int strafingDirection = -1;
    public float introWaitTime = 30.0f;

    public AudioSource strafeSound;
    private AudioClip leftStrafe;
    private AudioClip rightStrafe;

    private SteeringWheelControl wheelFunctions;
    void Start()
    {
        engineSound = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody2D>();
        bump = Resources.Load<AudioClip>("Audio/bumpend");
        leftStrafe = Resources.Load<AudioClip>("Audio/Car-Strafe-L");
        rightStrafe = Resources.Load<AudioClip>("Audio/Car-Strafe-R");
        movementDirection = transform.up;

        wheelFunctions = GetComponent<SteeringWheelControl>();
    }
    void Update()
    {
        // Discrete turn l/r 
        if(introWaitTime > 0.0f)
        {
            introWaitTime -= 1 * Time.deltaTime;
        }
        else
        {
            transform.position += movementDirection * movementSpeed;
        }
        //print(movementSpeed);
    }

    public void returnToNeutralSpeed()
    {
        if (Mathf.Abs(neutralSpeed - movementSpeed) < 0.01f)
        {
            movementSpeed = neutralSpeed;
            engineSound.pitch = 1;
        }
        else if (movementSpeed > neutralSpeed)
        {
            slowDown();
        }
        else
        {
            speedUp();
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
        if (introWaitTime > 0.0f) return;
        print(amount);

        if (amount < 0) strafingDirection = -1;
        else if (amount > 0) strafingDirection = 1;
        else amount = 0;

        strafeSound.volume = Mathf.Abs(amount);
        //strafeSound.volume = Mathf.Pow(Mathf.Abs(amount), 2);
        //strafeSound.panStereo = -strafeSound.volume;
        if (amount * lastRecordedStrafe <= 0)
        {
            if (amount < 0)
            {
                strafeSound.clip = leftStrafe;
                strafeSound.panStereo = 1;
            } else if (amount > 0)
            {
                strafeSound.clip = rightStrafe;
                strafeSound.panStereo = -1;
            }
            strafeSound.Play();
        }

        //when we have a squeaky wheel sound, we'll use this when the player turns the wheel left/right
        ////plays squeaky noise when wheel turns left/right
        //if (amount < lastRecordedStrafe && !strafeSound.isPlaying)
        //{
        //    strafeSound.clip = leftStrafe;
        //    strafeSound.Play();
        //}
        //else if (amount > lastRecordedStrafe && !strafeSound.isPlaying)
        //{
        //    strafeSound.clip = rightStrafe;
        //    strafeSound.Play();
        //}
        //else
        //{
        //    strafeSound.Stop();
        //}

        //stops car from going too far left/right
        if (invalidDirection / amount > 0)
        {
            //print("HITTING RAIL" + amount);
            wheelFunctions.PlaySoftstopForce(1);
            if (lastRecordedStrafe == 0 || amount / lastRecordedStrafe <= 1f)
            {
                lastRecordedStrafe = amount;
            }
            return;
        }
        else { wheelFunctions.StopSoftstopForce(); }

        //print(lastRecordedStrafe);
        lastRecordedStrafe = amount;
        //moves car left/right
        transform.position += amount * movementSpeed * transform.right;
    }

    //this is linked to the steering wheel in the UI.
    public float GetWheelAngle()
    {
        return lastRecordedStrafe * -443;
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
