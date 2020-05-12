using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBumpSound : MonoBehaviour
{
    public AudioSource enterAudio;
    public AudioSource exitAudio;
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

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Played bump sound");
        if (col.gameObject.tag == "Player")
        {
            enterAudio.Play();
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("Played bump sound");
        if (col.gameObject.tag == "Player")
        {
            exitAudio.Play();
            Debug.Log("Played bump sound");
        }
    }
}
