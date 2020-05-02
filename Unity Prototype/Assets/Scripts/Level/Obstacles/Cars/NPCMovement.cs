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
    private float acceleration = 0.02f;
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

    string IfFindsObstacleReturnTag(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, eyesight);
        if (hit.collider != null)
        {
            return hit.collider.gameObject.tag;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator Coast()
    {
        StopAllCoroutines();
        while (neutralSpeed > movementSpeed)
        {
            movementSpeed += acceleration;
            yield return new WaitForSeconds(0);
        }
        while (neutralSpeed < movementSpeed)
        {
            movementSpeed -= acceleration;
            yield return new WaitForSeconds(0);
        }
        movementSpeed = neutralSpeed;
    }

    public IEnumerator SwitchLaneRight(bool isRight)
    {
        StopAllCoroutines();
        int direction = 1;
        if (!isRight)
        {
            direction *= -1;
        }

        int laneWidth = 40;
        float endPositionX = transform.position.x + direction * laneWidth;
        while (endPositionX - transform.position.x > 0.02f)
        {
            transform.position += new Vector3(movementSpeed * direction,0,0);
            yield return new WaitForSeconds(0);
        }
    }

    // Update is called once per frame
    public IEnumerator speedUp()
    {
        StopAllCoroutines();
        while (movementSpeed < maxSpeed)
        {
            movementSpeed += 0.01f;
            yield return new WaitForSeconds(0);
        }
        movementSpeed = maxSpeed;
    }
    public IEnumerator slowDown()
    {
        StopAllCoroutines();
        while (movementSpeed > minSpeed)
        {
            movementSpeed *= 0.98f;
            yield return new WaitForSeconds(0);
        }
        movementSpeed = minSpeed;
    }
    public IEnumerator suddenStop()
    {
        StopAllCoroutines();
        while (movementSpeed > 0.01f)
        {
            movementSpeed *= 0.96f;
            yield return new WaitForSeconds(0);
        }
        movementSpeed = 0;
    }

    public void setSpeed(float speed)
    {
        movementSpeed = speed;
    }
}
