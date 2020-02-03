using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour
{
    private float eyesight = 2.5f;
    private float lastrecordeddistance = 2;
    public AudioSource beep;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D frontSight = Physics2D.Raycast(transform.position, transform.up, eyesight);
        //Debug.DrawRay(transform.position, transform.up * eyesight, Color.green);
        //print(hit.collider);
        beep.volume = 0;
        if (frontSight.collider && frontSight.collider.gameObject.tag == "Car")
        {
            float distance = frontSight.collider.gameObject.transform.position.y - transform.position.y;
            beep.volume = 1;
            beep.pitch = 0.33f*Mathf.Pow(distance-3, 2) + 1f;
            //beep.pitch = -(3/eyesight)*distance + 3 + 1;
        }
    }

    /*
     * 
        RaycastHit2D sightLeftSide = Physics2D.Raycast(transform.position - new Vector3(0.25f, 0, 0), transform.up, eyesight);
        RaycastHit2D sightRightSide = Physics2D.Raycast(transform.position + new Vector3(0.25f, 0, 0), transform.up, eyesight);
        //checks the front of the car on the left and right. If the beeping doesn't play, the car is free to move ahead.
        //beep.volume = 0;
        float leftDistance = 1;
        float rightDistance = 1;
        if (sightLeftSide.collider.gameObject.tag == "Car")
        {
            leftDistance = Vector3.Magnitude(sightLeftSide.collider.gameObject.transform.position - transform.position);
        }
        if (sightRightSide.collider.gameObject.tag == "Car")
        {
            rightDistance = Vector3.Magnitude(sightRightSide.collider.gameObject.transform.position - transform.position);
        }
        float distance = Mathf.Min(leftDistance, rightDistance);
        //beep.volume = 1;
        beep.pitch = 0.333f * Mathf.Pow(distance - 3, 2) + 1f;
        //beep.pitch = -(3/eyesight)*distance + 3 + 1;
        */
}
