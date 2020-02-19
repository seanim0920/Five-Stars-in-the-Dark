using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silence : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print(col.gameObject.name);
        if (col.gameObject.GetComponent<AudioSource>())
        {
            // Debug.Log(" collision detected from tire ");
            col.gameObject.GetComponent<AudioSource>().Pause();
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<AudioSource>())
        {
            col.gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
