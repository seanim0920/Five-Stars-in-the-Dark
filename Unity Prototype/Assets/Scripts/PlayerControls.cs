﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerControls : MonoBehaviour
{
    public AudioSource engineSound;
    public AudioSource tireSound;
    private float movementSpeed = 0f;
    private float maxSpeed = 0.08f;
    private float neutralSpeed = 0.05f;
    private float acceleration = 0.001f;
    private Rigidbody2D body;
    private AudioClip bump;
    private Vector3 movementDirection;
    private int blockedSide = 0;
    private float lastRecordedStrafe = 0;
    private int strafingDirection = -1;
    public AudioSource leftStrafe;
    public AudioSource rightStrafe;

    public AudioSource strafeSound;

    public AudioMixer leftSpeaker;
    public AudioMixer rightSpeaker;

    private SteeringWheelControl wheelFunctions;
    public AudioMixer slowinstruments;

    private string[] instruments = { "Drums", "Bass", "Keyboard", "Lead", "Wind", "Support"};

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        bump = Resources.Load<AudioClip>("Audio/bumpend");
        movementDirection = transform.up;

        wheelFunctions = GetComponent<SteeringWheelControl>();
    }
    void Update()
    {
        for (int loop = 0; loop < 2; loop++)
        {
            AudioMixer speaker = leftSpeaker;
            if (loop == 0) speaker = rightSpeaker;
            float speed = (movementSpeed / maxSpeed);
            for (int insname = 0; insname < 6; insname++)
            {
                if (insname <= 2)
                    speaker.SetFloat(instruments[insname] + "Volume", -60 * (Mathf.Pow((speed-1), ((insname + 1 + 2)*2))));
                else if (insname == 3)
                    speaker.SetFloat(instruments[insname] + "Volume", 60 * (speed - 1));
                else
                    speaker.SetFloat(instruments[insname] + "Volume", 60 * (Mathf.Pow(speed, insname) - 1));
            }
        }

        engineSound.volume = -Mathf.Pow((movementSpeed / maxSpeed), 2) + 1;
        tireSound.volume = Mathf.Pow((movementSpeed / maxSpeed), 2);
        leftStrafe.volume = tireSound.volume/2;
        rightStrafe.volume = tireSound.volume/2;
        wheelFunctions.PlayDirtRoadForce((int)(Mathf.Pow((movementSpeed/maxSpeed),2) * 16));
        // Discrete turn l/r 
        transform.position += movementDirection * movementSpeed;
        //print(movementSpeed);

//        slowinstruments.SetFloat("DrumsVolume", movementSpeed/maxSpeed);
    }

    public void returnToNeutralSpeed()
    {
        if (Mathf.Abs(neutralSpeed - movementSpeed) < 0.005f)
        {
            movementSpeed = neutralSpeed;
            //setRadioTempo(1f);
        }
        else if (movementSpeed > neutralSpeed)
        {
            slowDown(0.05f);
        }
        else
        {
            speedUp(1f);
        }
    }

    public void slowDown(float amount)
    {
        movementSpeed *= 1-amount;
        //setRadioTempo(getRadioTempo()*(1-amount));
    }
    public void speedUp(float amount)
    {
        if (movementSpeed < maxSpeed)
        {
            movementSpeed += acceleration*amount;
            //setRadioTempo(getRadioTempo() + acceleration*amount/neutralSpeed);
        }
    }
    public void blockDirection(int direction)
    {
        blockedSide = direction;
    }
    public void strafe(float amount) //amount varies between -1 (steering wheel to the left) and 1 (steering wheel to the right)
    {
        //print(amount);

        if (amount < 0) strafingDirection = -1;
        else if (amount > 0) strafingDirection = 1;
        else amount = 0;

        tireSound.panStereo = amount;
        //strafeSound.volume = Mathf.Abs(amount)*3;
        ////strafeSound.volume = Mathf.Pow(Mathf.Abs(amount), 2);
        ////strafeSound.panStereo = -strafeSound.volume;
        //if (amount * lastRecordedStrafe <= 0)
        //{
        //    if (amount < 0)
        //    {
        //        strafeSound.clip = leftStrafe;
        //        strafeSound.panStereo = 1;
        //    } else if (amount > 0)
        //    {
        //        strafeSound.clip = rightStrafe;
        //        strafeSound.panStereo = -1;
        //    }
        //    strafeSound.Play();
        //}

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
        if (blockedSide / amount > 0)
        {
            //print("HITTING RAIL" + amount);
            wheelFunctions.PlaySoftstopForce(1);
            if (lastRecordedStrafe == 0 || amount / lastRecordedStrafe <= 1f)
            {
                lastRecordedStrafe = amount;
            }
            return;
        }
        else if (wheelFunctions) { 
            wheelFunctions.StopSoftstopForce();
        }

        //print(lastRecordedStrafe);
        lastRecordedStrafe = amount;
        //moves car left/right
        if (Mathf.Abs(amount) > 0.02f)
            transform.position += amount * 2 * movementSpeed * transform.right;
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
            //setRadioTempo(getRadioTempo() * 0.1f);
        }
    }

    //public void setRadioTempo(float speed)
    //{
    //    radio.SetFloat("Speed", speed);
    //    radio.SetFloat("Pitch", 1 / speed);
    //    //radio.SetFloat("Pitch", -(speed - 4) / 3);
    //    if (speed < 0.01f) radio.SetFloat("Pitch", 0);
    //}
    //public float getRadioTempo()
    //{
    //    float speed = 0;
    //    radio.GetFloat("Speed", out speed);
    //    return speed;
    //}
}
