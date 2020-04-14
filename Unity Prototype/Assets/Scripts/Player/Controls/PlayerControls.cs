using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerControls : MonoBehaviour
{
    public AudioSource engineSound;
    // private Transform engineSounds;
    public AudioSource tireSound;
    public float movementSpeed = 0f;
    public float minSpeed = 0.01f;
    public float maxSpeed = 0.08f;
    public float neutralSpeed = 0.05f;
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
    public AudioMixer engineMixer;
    public AudioMixerSnapshot[] engineSounds;
    [SerializeField] private float[] snapshotWeights;

    private SteeringWheelControl wheelFunctions;
    public AudioMixer slowinstruments;

    private string[] instruments = { "Lead", "Bass", "Keyboard", "Wind", "Support", "Drums"};

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
        wheelFunctions.PlayDirtRoadForce((int)(Mathf.Pow((1-(movementSpeed/maxSpeed)),1) * 25));
        // Discrete turn l/r 
        transform.position += movementDirection * movementSpeed;
        //print(movementSpeed);

//        slowinstruments.SetFloat("DrumsVolume", movementSpeed/maxSpeed);
    }

    public void returnToNeutralSpeed()
    {
        // Transform engineSounds = engineSound.transform; // Get Engine children
        // Debug.Log("Inside returnToNeutralSpeed");
        if (Mathf.Abs(neutralSpeed - movementSpeed) < 0.005f)
        {
            // Play Coasting Clip
            if(!engineSound.transform.GetChild(0).GetComponent<AudioSource>().isPlaying)
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
            slowDown(0.005f);
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
        if (movementSpeed <= minSpeed) return;
        // Play Slowing Down Clip
        if(!engineSound.transform.GetChild(2).GetComponent<AudioSource>().isPlaying)
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

        movementSpeed *= 1-amount;
        //setRadioTempo(getRadioTempo()*(1-amount));
    }
    public void speedUp(float amount)
    {
        // Play Accelerating Clip
        if(!engineSound.transform.GetChild(1).GetComponent<AudioSource>().isPlaying)
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
        if (amount < 0) strafingDirection = -1;
        else if (amount > 0) strafingDirection = 1;
        else strafingDirection = 0;

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

        //print(blockedSide / amount);
        if (blockedSide / amount > 0)
        {
            if (amount < -0.02f) wheelFunctions.PlaySideCollisionForce(-100);
            else if (amount > 0.02f) wheelFunctions.PlaySideCollisionForce(100);
            print("HITTING RAIL" + amount);
            if (lastRecordedStrafe == 0 || amount / lastRecordedStrafe <= 1f)
            {
                lastRecordedStrafe = amount;
            }
            return;
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
        switch(transitionNum)
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
