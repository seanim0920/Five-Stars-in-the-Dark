using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private Rigidbody2D body;
    private AudioClip bump;
    public GameObject hitsound;
    private Control1D controlFunctions;
    // Start is called before the first frame update
    void Start()
    {
        bump = Resources.Load<AudioClip>("Audio/hit");
        controlFunctions = GetComponent<Control1D>();
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
            CheckErrors.IncrementErrorsAndUpdateDisplay();
        }
        if (col.gameObject.tag == "Curb")
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f);
            if (hit.collider != null && hit.collider.gameObject.tag == "Curb")
            {
                controlFunctions.blockDirection(1);
            } else
            {
                print("else");
                controlFunctions.blockDirection(-1);
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
        if (col.gameObject.tag == "Curb")
        {
            controlFunctions.blockDirection(0);
        }
    }
}
