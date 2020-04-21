using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBumpSound : MonoBehaviour
{
    public AudioClip enterAudio;
    public AudioClip exitAudio;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            source.PlayOneShot(enterAudio);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            source.PlayOneShot(exitAudio);
        }
    }
}
