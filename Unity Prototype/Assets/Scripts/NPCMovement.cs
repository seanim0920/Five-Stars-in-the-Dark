using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float movementSpeed = 0.05f;
    private float maxSpeed = 0.1f;
    private float neutralSpeed = 0.05f;
    private float acceleration = 0.001f;
    private AudioSource engineSound;
    private float eyesight = 3;
    private Vector3 movementDirection;
    void Start()
    {
        engineSound = GetComponent<AudioSource>();
        StartCoroutine(SwitchLanes());
        movementDirection = transform.up;
    }
    void Update()
    {
        if (SeesObstacle(transform.up))
        {
            movementSpeed *= 0.92f;
            engineSound.pitch *= 0.92f;
        }
        else if (SeesObstacle(transform.right))
        {
            movementSpeed += acceleration;
            engineSound.pitch += acceleration / neutralSpeed;
        }
        else
        {
            if (neutralSpeed > movementSpeed + 0.02f)
            {
                movementSpeed += acceleration;
                engineSound.pitch += acceleration / neutralSpeed;
            }
            else if (neutralSpeed < movementSpeed - 0.02f)
            {
                movementSpeed -= acceleration;
                engineSound.pitch -= acceleration / neutralSpeed;
            }
            else
            {
                movementSpeed = neutralSpeed;
                engineSound.pitch = 1f;
            }
        }

        transform.position += movementDirection * movementSpeed;
    }

    bool SeesObstacle(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, eyesight);
        if (hit.collider != null && (hit.collider.gameObject.tag == "Stop" || hit.collider.gameObject.tag == "Car" || hit.collider.gameObject.tag == "Player"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    IEnumerator SwitchLanes()
    {
        //randomly switch to the right or left lane
        while (true)
        {
            //how will the car figure out where to go? just check to the right or left to see if there is open space/curb
            yield return new WaitForSeconds(Random.Range(1.0f, 25.0f));

            Vector3 direction = transform.right;
            if (Random.Range(0, 1) == 0)
            {
                direction *= -1;
            }

            //2 is the size of a lane
            Vector3 goal = transform.position + direction * 2;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, eyesight);
            if (!SeesObstacle(direction) && hit.collider.gameObject.tag != "Guardrail")
            {
                while ((goal - transform.position).magnitude > 0.02f)
                {
                    //0.02f is movement speed while strafing
                    transform.position += (goal - transform.position).normalized * 0.02f;
                    yield return null;
                }
            }
        }
    }

    IEnumerator Honk()
    {
        yield return null;
    }

    public void setSpeed(float speed)
    {
        movementSpeed = speed;
    }
}
