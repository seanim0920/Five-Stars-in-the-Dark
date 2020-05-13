using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOnObjectCollision : MonoBehaviour
{
    //This script decideds what to do when the Character collides with an object, depending one what that object is tagged as
    //Right now it's used to play sounds, but it could be used for other things later

    AudioSource[] characterSounds;
    AudioSource charOnPed;
    AudioSource charOnGuard;
    AudioSource charOnCar;
        
    // Start is called before the first frame update
    void Start()
    {
        //gets the sounds from whatever gameobject this script is attached to
        characterSounds = GetComponents<AudioSource>();

        //checks to see if there are at least 3 audio sources
        if(characterSounds.Length < 3)
        {
            Debug.Log("The main character is missing audio sources. Collision SFX may not behave properly");
        }

        //Remember to ask Sound to start all collision audio with CoX, where X is C, G, or P depending on what is being crashed into
        charOnPed = ComponentAudioSearch('P');
        charOnGuard = ComponentAudioSearch('G');
        charOnCar = ComponentAudioSearch('C');
    }

    // OnCollisionEnter2d is called when an incoming collider makes contact with this object's collider (2D physics only)
    void OnCollisionEnter2D(Collision2D collision)
    {
        //again, collision is the object being run into, not the character
        //This method will be a controller for other methods that happen when the player collides with something

        PlayCollisionAudio(collision.gameObject.tag);
    }

    void PlayCollisionAudio(string collideTag)
    {
        switch (collideTag)
        {
            //.Play() is the method that actually plays the sound. Most everything else will be done in editor.
            case "Pedestrain":
                charOnPed.Play();
                break;

            case "Stop":
                charOnPed.Play();
                break;

            case "Car":
                charOnCar.Play();
                break;

            case "Gaurdrail":
                charOnGuard.Play();
                break;
            default:
                //Nothing happens if there's no associated sound to play
                break;
        }
    }


    //This method locates audioclips based on the convention 'CoX', where X is the character of the type of thing being collided with
    AudioSource ComponentAudioSearch(char findme)
    {
        AudioSource target = null;

        for(int i = 0; i <= characterSounds.Length - 1; ++i)
        {
            //If CoX is followed, X will always be in the 2nd index
            if(characterSounds[i].clip.name[2] == findme)
            {
                target = characterSounds[i];
                i = characterSounds.Length + 3;
            }
        }

        if(target == null)
        {
            //Keep in mind that, if this method can't find the clip, the reference is set to null. This will cause problems.
            Debug.Log("Audio clip for collision type " + findme + " couldn't be found. Collision SFX may not behave properly");
        }

        return target;
    }
    
}
