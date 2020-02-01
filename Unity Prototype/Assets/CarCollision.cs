using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private Rigidbody2D body;
    private AudioClip bump;
    public GameObject hitsound;
    // Start is called before the first frame update
    void Start()
    {
        bump = Resources.Load<AudioClip>("Audio/hit");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            hitsound.transform.position = transform.position;
            hitsound.GetComponent<AudioSource>().Play();
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            hitsound.transform.position = transform.position;
            hitsound.GetComponent<AudioSource>().Play();
        }
    }
}
