using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private Rigidbody2D body;
    private AudioSource bump;
    // Start is called before the first frame update
    void Start()
    {
        bump = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        bump.Play();
    }
    void OnTriggerExit2D(Collider2D col)
    {
        bump.Play();
    }
}
