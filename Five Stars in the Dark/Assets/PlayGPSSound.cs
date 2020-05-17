using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGPSSound : MonoBehaviour
{
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            source.Play();
        }
    }
}
