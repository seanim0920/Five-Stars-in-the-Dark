using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement1D : MonoBehaviour
{
    private float movementSpeed = 0.03f;
    private float maxSpeed = 0.1f;
    private float neutralSpeed = 0.03f;
    private float acceleration = 0.001f;
    private AudioSource engineSound;
    private float eyesight = 3;
    void Start()
    {
        engineSound = GetComponent<AudioSource>();
        StartCoroutine(SwitchLanes());
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
            if (neutralSpeed - movementSpeed > 0.002f)
            {
                movementSpeed += acceleration;
                engineSound.pitch += acceleration / neutralSpeed;
            }
            else if (neutralSpeed - movementSpeed < -0.002f)
            {
                movementSpeed -= acceleration;
                engineSound.pitch -= acceleration / neutralSpeed;
            }
            else
            {
                movementSpeed = neutralSpeed;
                engineSound.pitch = 0.8f;
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

            RaycastHit2D hit = Physics2D.Raycast(transform.position, (new Vector3(5,transform.position.y + 1,0)) - transform.position, eyesight + 1);
            if (hit.collider == null || (hit.collider.gameObject.tag == "Curb"))
            {
                if (transform.position.x < 5)
                {
                    while (transform.position.x < 6)
                    {
                        transform.position += new Vector3(0.05f, 0, 0);
                        yield return new WaitForSeconds(0);
                    }
                } else
                {
                    while (transform.position.x > 4)
                    {
                        transform.position -= new Vector3(0.05f, 0, 0);
                        yield return new WaitForSeconds(0);
                    }
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
