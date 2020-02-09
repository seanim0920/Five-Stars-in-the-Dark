using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurbCollision : MonoBehaviour
{
    public AudioClip bump;
    public AudioClip bumpend;
    AudioSource audioData;
    Rigidbody2D carBody;

    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        carBody = gameObject.GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        audioData.volume = carBody.velocity.magnitude * 2;
        audioData.pitch = carBody.velocity.magnitude * 3f;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Curb")
        {
            // Debug.Log(" collision detected from tire ");
            audioData.Play();
            AudioSource.PlayClipAtPoint(bump, transform.position);

            // Increment error counter
            CheckErrors.IncrementErrorsAndUpdateDisplay();
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Curb")
        {
            AudioSource.PlayClipAtPoint(bumpend, transform.position);
            // Debug.Log(" collision finished from tire ");
            audioData.Stop();
        }
    }
}
