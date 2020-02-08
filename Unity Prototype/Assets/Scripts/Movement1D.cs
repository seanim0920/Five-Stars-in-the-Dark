using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement1D : MonoBehaviour
{
    public float movementSpeed = 0.05f;
    private float maxSpeed = 0.1f;
    private float neutralSpeed = 0.05f;
    private float acceleration = 0.001f;
    private AudioSource engineSound;
    private float eyesight = 3;
    void Start()
    {
        engineSound = GetComponent<AudioSource>();
        StartCoroutine(SwitchLanes());
        StartCoroutine(SpeedUp());
    }
    void Update()
    {
        if (SeesObstacle())
        {
            movementSpeed *= 0.92f;
            engineSound.pitch *= 0.92f;
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

        transform.position += transform.up * movementSpeed;
    }

    bool SeesObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, eyesight);
        //Debug.DrawRay(transform.position, transform.up * eyesight, Color.green);
        //print(hit.collider);
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
        //switch lanes at random
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.0f, 25.0f));

            int direction = Random.Range(0, 3);
            int goal = 6;

            if (direction == 0)
            {
                goal = 4;
            }
            if (direction == 1)
            {
                goal = 5;
            }
            if (direction == 2)
            {
                goal = 6;
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(goal, transform.position.y + movementSpeed, 0) - transform.position, eyesight + 1);
            if (hit.collider == null || (hit.collider.gameObject.tag == "Curb"))
            {
                while (goal - transform.position.x > 0.02f)
                {
                    RaycastHit2D check = Physics2D.Raycast(transform.position, new Vector3(goal, transform.position.y + movementSpeed, 0) - transform.position, eyesight + 1);
                    if (check.collider == null || (check.collider.gameObject.tag == "Curb"))
                    {
                        transform.position += new Vector3(((goal - transform.position.x)/Mathf.Abs(goal - transform.position.x))*0.02f, 0, 0);
                    }
                    yield return null;
                }
            }

            yield return new WaitForSeconds(Random.Range(0.0f, 3.0f));

        }
    }

    IEnumerator SpeedUp()
    {
        //speed up at random
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.0f, 25.0f));

            if (!SeesObstacle())
            {
                for (int i = 0; i < 120; i++)
                {
                    movementSpeed += acceleration;
                    engineSound.pitch += acceleration / neutralSpeed;
                }
            }

            yield return new WaitForSeconds(Random.Range(0.0f, 3.0f));

        }
    }

    public void setSpeed(float speed)
    {
        movementSpeed = speed;
    }
}
