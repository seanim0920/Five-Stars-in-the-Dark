using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    //public int direction;
    private AudioSource source;
    private AudioClip distance;
    private AudioClip direction;
    // Start is called before the first frame update
    void Start()
    {
        // ...
        source = GetComponent<AudioSource>();
        distance = Resources.Load<AudioClip>("Audio/gps200");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.parent.tag == "Right")
        {
            direction = Resources.Load<AudioClip>("Audio/gpsright");
        }
        else
        {
            direction = Resources.Load<AudioClip>("Audio/gpsleft");
        }
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(PlayWarnings());
        }
    }
    IEnumerator PlayWarnings()
    {
        source.PlayOneShot(distance);
        yield return new WaitForSeconds(distance.length);
        source.PlayOneShot(direction);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // ...
    }

    void OnTriggerExit2D(Collider2D other)
    {
    }
}