using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>  
///  Controller for car speed, turn and stopping.
/// </summary>  
public class Movement2D : MonoBehaviour {
    public float movementSpeed = -1f;
    public float angularSpeed = -1f;
    public Transform carTransform;
    public Rigidbody2D body;
    Vector2 direction = new Vector2();
    float angle = 0;
    public Transform turn;
    public AudioSource turnSound;
    Vector3 turnOffset = new Vector3();

    void Update() {
        // Discrete turn l/r 
        direction = new Vector2(transform.up.x, transform.up.y);
        if (Input.GetKey("up"))
        {
            body.AddForce(direction * movementSpeed);
        } else if (Input.GetKey("down"))
        {
            body.AddForce(-direction * movementSpeed);
        }
        if (Input.GetKeyDown("right"))
        {
            turnSound.Play();
            turnOffset = transform.right * 2;
        }
        else if (Input.GetKeyDown("left"))
        {
            turnSound.Play();
            turnOffset = transform.right * -2;
        }
        if (Input.GetKey("left") || Input.GetKey("right"))
        {
            turn.position = transform.position + turnOffset;
        }
        body.velocity = Vector2.ClampMagnitude(body.velocity, movementSpeed);
        if (Input.GetKey("left"))
        {
            if (angle < 90)
            {
                carTransform.Rotate(0, 0, angularSpeed, Space.Self);
            }
            angle += angularSpeed;
            if (angle == 90)
            {
                turnSound.Play();
            }
        }
        if (Input.GetKey("right"))
        {
            if (angle > -90)
            {
                carTransform.Rotate(0, 0, -angularSpeed, Space.Self);
            }
            angle -= angularSpeed;
            if (angle == -90)
            {
                turnSound.Play();
            }
        }
        if (!Input.GetKey("right") && !Input.GetKey("left"))
        {
            if (Mathf.Abs(angle) > 0 && Mathf.Abs(angle) < 90)
            {
                if (angle > 0)
                {
                    carTransform.Rotate(0, 0, 45 - angle, Space.Self);
                }
                else
                {
                    carTransform.Rotate(0, 0, -45 - angle, Space.Self);
                }
            }
            angle = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Building") {
            ContactPoint2D contact = collision.GetContact(0);

            body.AddForce(contact.normal * 15, ForceMode2D.Impulse);
        }
    }
}