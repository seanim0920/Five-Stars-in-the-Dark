using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public AudioSource honk;
    public AudioSource engineSound;
    public float movementSpeed = 0f;
    public float neutralSpeed = 0.05f;
    public float maxSpeed;
    public float minSpeed;
    private float acceleration = 0.01f;
    private float eyesight = 3;
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

        int laneWidth = 40;
        float endPositionX = transform.position.x + direction * laneWidth;
        while (endPositionX - transform.position.x > 0.02f)
        {
            transform.position += new Vector3(strafeSpeed * direction,0,0);
            yield return new WaitForSeconds(0);
        }
    }

    // Update is called once per frame
    public IEnumerator speedUp()
    {
        while (movementSpeed < maxSpeed)
        {
            movementSpeed += 0.01f;
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
        movementSpeed = minSpeed;
    }
    public IEnumerator suddenStop()
    {
        while (movementSpeed > 0.01f)
        {
            movementSpeed *= 0.96f;
            yield return new WaitForFixedUpdate();
        }
        movementSpeed = 0;
    }

    public void setSpeed(float speed)
    {
        movementSpeed = speed;
    }
}
