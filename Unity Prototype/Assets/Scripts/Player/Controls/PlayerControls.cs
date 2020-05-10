﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public AudioSource disabledWheelSound;
    public AudioSource engineSound;
    public AudioSource tireSound;
    public AudioSource dialogue;
    public AudioSource slidingSound;
    public AudioSource grabWheel;
    public float minSpeed = 0f;
    public float maxSpeed = 1.5f;
    public float neutralSpeed = 1f;
    public float acceleration = 0.01f;
    public float movementSpeed = 0f;

    private Rigidbody2D body;
    private AudioClip bump;
    private Vector3 movementDirection;
    private int blockedSide = 0;
    private static float lastRecordedStrafe = 0;
    private int strafingDirection = -1;

    public bool isTurning = false;
    public AudioSource strafeSound;
    public AudioMixer engineMixer;
    public AudioMixerSnapshot[] engineSounds;

    [Header("Private Attributes (visible for debugging)")]
    [SerializeField] private float[] snapshotWeights;

    private SteeringWheelControl wheelFunctions;

    private string[] instruments = { "Lead", "Bass", "Keyboard", "Wind", "Support", "Drums" };

    void Start()
    {
        // engineSounds = engineSound.transform;
        body = GetComponent<Rigidbody2D>();
        bump = Resources.Load<AudioClip>("Audio/bumpend");
        movementDirection = transform.up;

        // AudioMixerSnapshot[] engineSounds = {restToCoast, coastToAccel};

        wheelFunctions = GetComponent<SteeringWheelControl>();
    }
    void FixedUpdate()
    {
        //for (int loop = 0; loop < 2; loop++)
        //{
        //    AudioMixer speaker = leftSpeaker;
        //    if (loop == 0) speaker = rightSpeaker;
        //    float speed = (movementSpeed / maxSpeed);
        //    for (int insname = 0; insname < 6; insname++)
        //    {
        //        if (insname <= 2)
        //            speaker.SetFloat(instruments[insname] + "Volume", -60 * (Mathf.Pow((speed-1), ((insname + 1 + 2)*2))));
        //        else if (insname == 3)
        //            speaker.SetFloat(instruments[insname] + "Volume", 60 * (speed - 1));
        //        else
        //            speaker.SetFloat(instruments[insname] + "Volume", 60 * (Mathf.Pow(speed, insname) - 1));
        //    }
        //}

        engineSound.volume = -Mathf.Pow((movementSpeed / maxSpeed), 2) + 1;
        tireSound.volume = Mathf.Pow((movementSpeed / maxSpeed), 2);
        strafeSound.volume = tireSound.volume / 2;
        //wheelFunctions.PlayDirtRoadForce((int)(Mathf.Pow((1-(movementSpeed/maxSpeed)),1) * 25));
        // Discrete turn l/r 
        transform.position += movementDirection * movementSpeed;
        //print(movementSpeed);
    }

    public void returnToNeutralSpeed()
    {
        // Transform engineSounds = engineSound.transform; // Get Engine children
        // Debug.Log("Inside returnToNeutralSpeed");
        if (Mathf.Abs(neutralSpeed - movementSpeed) < 0.005f)
        {
            // Play Coasting Clip
            if (!engineSound.transform.GetChild(0).GetComponent<AudioSource>().isPlaying)
            {
                // engineSound.transform.GetChild(0).GetComponent<AudioSource>().volume = 0.667f;
                // Debug.Log("Coasting Clip");
                engineSound.transform.GetChild(0).GetComponent<AudioSource>().Play(); // Play Coasting sound
            }

            // Blend from whatever to only Coasting
            BlendSnapshot(1, 0.5f);
            movementSpeed = neutralSpeed;
            //setRadioTempo(1f);
        }
        else if (movementSpeed > neutralSpeed)
        {
            // Blend form MaxSpeed to Coasting
            // Debug.Log("MaxSpeed->Coasting");
            BlendSnapshot(3, 0.5f);
            slowDown(0.001f);
        }
        else
        {
            // Blend from Rest to Coasting
            // Debug.Log("Rest->Coasting");
            BlendSnapshot(0, 1.5f);
            speedUp(0.1f);
        }
    }

    public void slowDown(float amount)
    {
        if (!this.enabled)
        {
            if (brakeStart)
            {
                disabledAccel.Play();
            }
            return;
        }
        if (movementSpeed <= minSpeed) return;
        // Play Slowing Down Clip
        if (!engineSound.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
        {
            // Debug.Log("Slowing Clip");
            engineSound.transform.GetChild(2).GetComponent<AudioSource>().Play();
        }


        // Blend from Coasting to Rest
        if (movementSpeed <= neutralSpeed)
        {
            // Debug.Log("Coasting->Rest");
            BlendSnapshot(4, 4f);
        }

        movementSpeed *= 1 - amount;
        //setRadioTempo(getRadioTempo()*(1-amount));
    }
    public void speedUp(float amount)
    {
        if (!this.enabled)
        {
            if (accelStart)
            {
                disabledAccel.Play();
            }
            return;
        }
        // Play Accelerating Clip
        if (!engineSound.transform.GetChild(1).GetComponent<AudioSource>().isPlaying)
        {
            // Debug.Log("Accel Clip");
            engineSound.transform.GetChild(1).GetComponent<AudioSource>().PlayScheduled(10.5f);
        }

        // Blend from Coasting to Max Speed
        if (movementSpeed > neutralSpeed)
        {
            // Debug.Log("Coasting->MaxSpeed");
            BlendSnapshot(2, 0.5f);
        }
        else
        {
            // Blend from Rest to Coasting
            // Debug.Log("Rest->Coasting");
            BlendSnapshot(0, 0.5f);
        }

        if (movementSpeed < maxSpeed)
        {
            movementSpeed += acceleration * amount;
            //setRadioTempo(getRadioTempo() + acceleration*amount/neutralSpeed);
        }
    }
    public void blockDirection(int direction)
    {
        blockedSide = direction;
    }

    public void strafe(float amount) //amount varies between -1 (steering wheel to the left) and 1 (steering wheel to the right)
    {
        panCarSounds(amount);

        if (!this.enabled)
        {
            if (!isTurning && Mathf.Abs(amount) > 0)
            {
                print("trying to turn right");
                StartCoroutine(turnFail(amount > 0));
            }
            return;
        }

        //check if the player has begun turning (may not work for gamepad)
        if (!isTurning && Mathf.Abs(amount) > Mathf.Abs(lastRecordedStrafe))
        {
            isTurning = true;
            if (!slidingSound.isPlaying)
            {
                slidingSound.Play();
            }
            slidingSound.panStereo = amount * 3f;
        }
        else if (isTurning && Mathf.Abs(amount) < Mathf.Abs(lastRecordedStrafe))
        {
            isTurning = false;
            if (slidingSound.isPlaying && Mathf.Abs(amount) > 0.02f)
            {
                grabWheel.Play();
            }
            slidingSound.Stop();
        }

        //prevents car from moving if it's only nudged left/right
        //moves car to the side if there is no curb
        if (blockedSide / amount > 0)
        {
            if (amount < 0) wheelFunctions.PlaySideCollisionForce(-100);
            else if (amount > 0) wheelFunctions.PlaySideCollisionForce(100);
            //print("HITTING RAIL" + amount);
            if (lastRecordedStrafe == 0 || amount / lastRecordedStrafe <= 1f)
            {
                lastRecordedStrafe = amount;
            }
            return;
        }
        else if (Mathf.Abs(amount) > 0.01f)
        {
            transform.position += amount * (movementSpeed) * transform.right;
        }

        print("updating strafe");
        lastRecordedStrafe = amount;
    }

    private void panCarSounds(float amount)
    {
        engineSound.panStereo = amount * 3;
        foreach (Transform child in engineSound.gameObject.transform)
        {
            child.gameObject.GetComponent<AudioSource>().panStereo = amount * 3;
        }
        dialogue.panStereo = -amount * 3;
        strafeSound.panStereo = amount * 2;
    }

    public IEnumerator turnFail(bool right)
    {
        isTurning = true;
        disabledWheelSound.Play();
        float inc = -0.005f;
        if (right)
        {
            inc = 0.005f;
        }
        for (int i = 0; i < 5; i++)
        {
            lastRecordedStrafe += inc;
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 5; i++)
        {
            lastRecordedStrafe -= inc;
            yield return new WaitForFixedUpdate();
        }
        isTurning = false;
    }

    public static float getStrafeAmount()
    {
        return lastRecordedStrafe;
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

    // This function blends audio mixer snapshots together
    // Code was modified from the Unity Audio Mixer Snapshots YouTube tutorial:
    // https://youtu.be/2nYyws0qJOM
    public void BlendSnapshot(int transitionNum, float blendTime)
    {
        // Snapshot indices are as follows:
        // 0: Rest
        // 1: Rest to Coasting
        // 2: Coasting
        // 3: MaxSpeed
        // 4: MaxSpeed to Coasting
        switch (transitionNum)
        {
            case 0: // Rest -> Coasting
                snapshotWeights[0] = 0.0f;
                snapshotWeights[1] = 1.0f;
                snapshotWeights[2] = 0.0f;
                snapshotWeights[3] = 0.0f;
                snapshotWeights[4] = 0.0f;
                engineMixer.TransitionToSnapshots(engineSounds, snapshotWeights, blendTime);
                break;
            case 1: // Just Coasting
                snapshotWeights[0] = 0.0f;
                snapshotWeights[1] = 0.0f;
                snapshotWeights[2] = 1.0f;
                snapshotWeights[3] = 0.0f;
                snapshotWeights[4] = 0.0f;
                engineMixer.TransitionToSnapshots(engineSounds, snapshotWeights, blendTime);
                break;
            case 2: // Coast -> Max Speed
                snapshotWeights[0] = 0.0f;
                snapshotWeights[1] = 0.0f;
                snapshotWeights[2] = 0.0f;
                snapshotWeights[3] = 1.0f;
                snapshotWeights[4] = 0.0f;
                engineMixer.TransitionToSnapshots(engineSounds, snapshotWeights, blendTime);
                break;
            case 3: // Max Speed -> Coast
                snapshotWeights[0] = 0.0f;
                snapshotWeights[1] = 0.0f;
                snapshotWeights[2] = 0.0f;
                snapshotWeights[3] = 0.0f;
                snapshotWeights[4] = 1.0f;
                engineMixer.TransitionToSnapshots(engineSounds, snapshotWeights, blendTime);
                break;
            case 4: // Coast -> Rest
                snapshotWeights[0] = 1.0f;
                snapshotWeights[1] = 0.0f;
                snapshotWeights[2] = 0.0f;
                snapshotWeights[3] = 0.0f;
                snapshotWeights[4] = 0.0f;
                engineMixer.TransitionToSnapshots(engineSounds, snapshotWeights, blendTime);
                break;
        }
    }
}
