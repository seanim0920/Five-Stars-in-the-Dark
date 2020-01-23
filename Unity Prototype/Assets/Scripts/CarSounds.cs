using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    AudioSource audioData;
    public Rigidbody2D body;
    public AudioSource roll;
    Vector3 turnOffset = new Vector3();
    public float previousAngle = 0;
    private void Awake()
    {
        audioData = GetComponent<AudioSource>();
        audioData.Play();
    }
    void Update()
    {
        roll.volume = body.velocity.magnitude * 0.75f;
    }

    public AudioClip bang;

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        AudioSource.PlayClipAtPoint(bang, contact.point);
    }
}
