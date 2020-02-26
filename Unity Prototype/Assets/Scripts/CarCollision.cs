using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    private Rigidbody2D body;
    //private AudioClip bump;
    //public GameObject hitsound;
    private Control1D controlFunctions;

    //collision sounds
    AudioSource[] characterSounds;
    AudioSource charOnPed;
    AudioSource charOnGuard;
    AudioSource charOnCar;

    // Start is called before the first frame update
    void Start()
    {
        //controlFunctions = GetComponent<Control1D>();
        //bump = Resources.Load<AudioClip>("Audio/hit");

        //gets the sounds from whatever gameobject this script is attached to
        characterSounds = GetComponents<AudioSource>();

        //checks to see if there are at least 3 audio sources
        if (characterSounds.Length < 3)
        {
            Debug.Log("The main character is missing audio sources. Collision SFX may not behave properly");
        }

        //Remember to ask Sound to start all collision audio with CoX, where X is C, G, or P depending on what is being crashed into
        charOnPed = ComponentAudioSearch('P');
        charOnGuard = ComponentAudioSearch('G');
        charOnCar = ComponentAudioSearch('C');
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            //hitsound.transform.position = transform.position;
            //hitsound.GetComponent<AudioSource>().Play();
            charOnCar.Play();
            CheckErrors.IncrementErrorsAndUpdateDisplay();
        }
        if (col.gameObject.tag == "Pedestrian" || col.gameObject.tag == "Stop")
        {
            charOnPed.Play();
        }
        if (col.gameObject.tag == "Guardrail")
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f);
            if (hit.collider != null && hit.collider.gameObject.tag == col.gameObject.tag)
            {
                controlFunctions.blockDirection(1);
            } else
            {
                print("else");
                controlFunctions.blockDirection(-1);
            }
            charOnGuard.Play();
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            //hitsound.transform.position = transform.position;
            //hitsound.GetComponent<AudioSource>().Play();
            charOnCar.Play();
        }
        if (col.gameObject.tag == "Guardrail")
        {
            controlFunctions.blockDirection(0);
            charOnGuard.Stop();
        }
    }

    //This method locates audioclips based on the convention 'CoX', where X is the character of the type of thing being collided with
    AudioSource ComponentAudioSearch(char findme)
    {
        AudioSource target = null;

        for (int i = 0; i <= characterSounds.Length - 1; ++i)
        {
            //If CoX is followed, X will always be in the 2nd index
            if (characterSounds[i].clip.name[2] == findme)
            {
                target = characterSounds[i];
                i = characterSounds.Length + 3;
            }
        }

        if (target == null)
        {
            //Keep in mind that, if this method can't find the clip, the reference is set to null. This will cause problems.
            Debug.Log("Audio clip for collision type " + findme + " couldn't be found. Collision SFX may not behave properly");
        }

        return target;
    }
}