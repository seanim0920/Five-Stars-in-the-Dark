using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public AudioSource honk;
    public AudioSource engineSound;
    public float movementSpeed = 0f;
    public float neutralSpeed = 1f;
    public float maxSpeed;
    public float minSpeed;
    private float acceleration = 0.01f;
    private float eyesight = 60;
    private Vector3 movementDirection;
    void Start()
    {
        StartCoroutine(Coast());
    }

    void FixedUpdate()
    {
        //keep refreshing movementdirection, car may rotate
        movementDirection = transform.up;

        transform.position += movementDirection * movementSpeed;
    }

    public GameObject SeesObstacle(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, eyesight);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator Coast()
    {
        while (movementSpeed < neutralSpeed)
        {
            movementSpeed += acceleration;
            yield return new WaitForFixedUpdate();
        }
        while (movementSpeed > neutralSpeed)
        {
            movementSpeed -= acceleration;
            yield return new WaitForFixedUpdate();
        }
        movementSpeed = neutralSpeed;
        print(movementSpeed);
    }

    public IEnumerator SwitchLaneRight(bool isRight, float strafeSpeed)
    {
        int direction = 1;
        if (!isRight)
        {
            direction *= -1;
        }

        int laneWidth = 30;
        float endPositionX = transform.position.x + direction * laneWidth;
        while (endPositionX > transform.position.x)
        {
            transform.position += new Vector3(strafeSpeed,0,0);
            yield return new WaitForFixedUpdate();
        }
        while (endPositionX < transform.position.x)
        {
            transform.position -= new Vector3(strafeSpeed, 0, 0);
            yield return new WaitForFixedUpdate();
        }
        transform.position = new Vector3(endPositionX,transform.position.y,transform.position.z);
    }

    // Update is called once per frame
    public IEnumerator speedUp()
    {
        while (movementSpeed < maxSpeed)
        {
            movementSpeed += acceleration;
            yield return new WaitForFixedUpdate();
        }
        movementSpeed = maxSpeed;
    }
    public IEnumerator slowDown()
    {
        while (movementSpeed > minSpeed)
        {
            movementSpeed *= 0.98f;
            yield return new WaitForFixedUpdate();
        }
        while (movementSpeed < minSpeed)
        {
            movementSpeed += acceleration/2;
            yield return new WaitForFixedUpdate();
        }
        movementSpeed = minSpeed;
    }
    public IEnumerator suddenStop()
    {
        while (movementSpeed > 0.01f)
        {
            //print("trying to stop car");
            movementSpeed *= 0.96f;
            yield return new WaitForFixedUpdate();
        }
        movementSpeed = 0;
    }
    public void resetMovement()
    {
        StopAllCoroutines();
    }

    public void setSpeed(float speed)
    {
        movementSpeed = speed;
    }
}
