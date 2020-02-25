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
    private float lastRecordedStrafe = 0;
    public float introWaitTime = 30.0f;

    public AudioSource wheelSound;
    private AudioClip leftTurn;
    private AudioClip rightTurn;

    private SteeringWheelControl wheelFunctions;
    void Start()
    {
        engineSound = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody2D>();
        bump = Resources.Load<AudioClip>("Audio/bumpend");
        leftTurn = Resources.Load<AudioClip>("Audio/lowSqueak");
        rightTurn = Resources.Load<AudioClip>("Audio/highSqueak");
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
        print(amount);
        //plays squeaky noise when wheel turns left/right
        if (amount < lastRecordedStrafe && !wheelSound.isPlaying)
        {
            wheelSound.clip = leftTurn;
            wheelSound.Play();
        }
        else if (amount > lastRecordedStrafe && !wheelSound.isPlaying)
        {
            wheelSound.clip = rightTurn;
            wheelSound.Play();
        }
        else
        {
            wheelSound.Stop();
        }
        //print(lastRecordedStrafe);
        lastRecordedStrafe = amount;

        //changes music depending on how far wheel is turned
        //print(amount);
        //engineSound.outputAudioMixerGroup.audioMixer.SetFloat("LowF", 19500*amount + 20000);
        //engineSound.outputAudioMixerGroup.audioMixer.SetFloat("HighF", 1700 * amount);
        engineSound.outputAudioMixerGroup.audioMixer.SetFloat("LowF", 20000*Mathf.Exp(3.689f*amount));
        engineSound.outputAudioMixerGroup.audioMixer.SetFloat("HighF", 1*Mathf.Exp(7.438f*amount));

        //wheelFunctions.PlaySoftstopForce(5);
        //stops car from going too far left/right
        if (invalidDirection / amount > 0)
        {
            //print("HITTING RAIL" + amount);
            wheelFunctions.PlaySoftstopForce(1);
            return;
        }
        else { wheelFunctions.StopSoftstopForce(); }

        //moves car left/right
        transform.position += amount * movementSpeed * transform.right;

        if (invalidDirection / amount > 0 || introWaitTime > 0.0f) return;
        transform.position += amount * 2 * movementSpeed * transform.right;
    }

    public float GetWheelAngle()
    {
        return lastRecordedStrafe * -442;
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
