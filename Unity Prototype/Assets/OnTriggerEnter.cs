using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnter : MonoBehaviour
{
    AudioSource warning;
    // Start is called before the first frame update
    void Start()
    {
        warning = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (tag == "Stop")
        {
            warning.Play();
        }
    }
}
