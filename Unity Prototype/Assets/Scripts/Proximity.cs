using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Proximity : MonoBehaviour
{
    private float eyesight = 4f;
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
        if (col.gameObject.tag == "Car")
        {
            Vector3 difference = (col.gameObject.transform.position - transform.parent.transform.position);
            float distance = difference.magnitude;
            noise.volume = Mathf.Pow(((-distance / transform.localScale.y) + 1), 2) * 1.1f;
            noise.panStereo = Vector3.Cross(difference, transform.up).magnitude * (Vector3.Dot(col.gameObject.transform.position - transform.parent.transform.position, transform.right) > 0 ? 1 : -1) / (transform.localScale.x / 2);
            noise.pitch = (1 / eyesight) * Mathf.Pow(distance - eyesight, 2) + 1;
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
