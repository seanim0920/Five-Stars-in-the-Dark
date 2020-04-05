using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMovement : MonoBehaviour
{
    public AudioSource siren;
    private float movementSpeed = 1f;
    public float neutralSpeed = 1f;
    private float maxSpeed = 1f;
    private float acceleration = 0f;
    // private AudioSource engineSound;
    // private float eyesight = 3;
    private Vector3 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        movementDirection = transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        drive();
    }

    void drive()
    {
        transform.position += movementDirection * movementSpeed;
    }
}
