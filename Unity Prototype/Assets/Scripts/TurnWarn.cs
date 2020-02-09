using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnWarn : MonoBehaviour
{
    private AudioSource source;
    private AudioClip direction;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
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
            source.PlayOneShot(direction);
        }
    }
}
