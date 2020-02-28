using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private Rigidbody2D body;
    public GameObject hitsound;
    private PlayerControls controlFunctions;
    public AudioSource guardAudio;

    // Start is called before the first frame update
    void Start()
    {
        controlFunctions = GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            hitsound.transform.position = col.gameObject.transform.position;
            hitsound.GetComponent<AudioSource>().Play();
            CheckErrors.IncrementErrorsAndUpdateDisplay();
        }
        if (col.gameObject.tag == "Guardrail")
        {
            guardAudio.Play();
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f);
            if (hit.collider != null && hit.collider.gameObject.tag == col.gameObject.tag)
            {
                controlFunctions.blockDirection(1);
                guardAudio.panStereo = 1;
            } else
            {
                controlFunctions.blockDirection(-1);
                guardAudio.panStereo = -1;
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            hitsound.transform.position = transform.position;
            hitsound.GetComponent<AudioSource>().Play();
        }
        if (col.gameObject.tag == "Guardrail")
        {
            controlFunctions.blockDirection(0);
            guardAudio.Stop();
        }
    }
}