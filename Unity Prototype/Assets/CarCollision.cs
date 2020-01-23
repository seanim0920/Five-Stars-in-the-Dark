using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private Rigidbody2D body;
    public AudioClip bump;
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
            AudioSource.PlayClipAtPoint(bump, col.gameObject.transform.position);
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
            AudioSource.PlayClipAtPoint(bump, col.gameObject.transform.position);
    }
}
