using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Proximity : MonoBehaviour
{
    AudioSource noise;
    private bool isBeep = true;
    // Start is called before the first frame update
    void Start()
    {
        noise = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            if (isBeep)
            {
                noise.clip = Resources.Load<AudioClip>("Audio/noise");
            } else
            {
                noise.clip = Resources.Load<AudioClip>("Audio/proximitybeep");
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //should be adjusted to detect the closest car to the player, if there are multiple cars in the zone
        if (col.gameObject.tag == "Car")
        {
            Vector3 difference = (col.gameObject.transform.position - transform.parent.transform.position);
            float distance = difference.magnitude;
            float eyesight = transform.localScale.y * transform.parent.transform.localScale.y;
            noise.volume = Mathf.Pow(((-distance / (eyesight)) + 1), 2) * 1.1f;
            //print("proximity volume is " + distance + " " + noise.volume);
            Vector3 posRelativeToPlayer = transform.parent.transform.InverseTransformPoint(col.gameObject.transform.position);
            noise.panStereo = posRelativeToPlayer.x / (transform.localScale.x / 2);
            noise.pitch = noise.volume * 3;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            noise.volume = 0;
        }
    }
}
