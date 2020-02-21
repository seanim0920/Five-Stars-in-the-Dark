using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour
{
    private float eyesight = 2;
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
            beep.pitch = 0.333f*Mathf.Pow(distance-3, 2) + 1;
            //beep.pitch = -(3/eyesight)*distance + 3 + 1;
        }
    }
}
