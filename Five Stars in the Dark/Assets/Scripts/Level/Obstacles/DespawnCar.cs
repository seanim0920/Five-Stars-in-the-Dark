using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnCar : MonoBehaviour
{
    private bool isDespawning = false;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // If player has crashed into this car
        if(gameObject.GetComponent<TutorialCarFailure>().hasPlayedFailure)
        {
            // If so, Destroy the car
            if(!isDespawning)
            {
                StartCoroutine(driveAway());
            }
        }
        // Check if player passed this car by x amount (probably the logarithmic rolloff)
        else if(player.transform.position.y - transform.position.y > transform.GetChild(0).GetComponent<AudioSource>().maxDistance / 2)
        {
            // Reset the position
            // Calculate x position based off of player strafe amount
            float xpos = player.transform.position.x + player.GetComponent<PlayerControls>().getStrafeAmount() * player.GetComponent<PlayerControls>().movementSpeed;
            transform.position = new Vector3(xpos,
                                                player.transform.position.y + 150,
                                                player.transform.position.z);
                                                // The +200 comes from the spawn distance in the level constructor
        }
    }

    IEnumerator driveAway()
    {
        isDespawning = true;
        Vector3 exitRotation = new Vector3(0, 0, 0);
        // Quaternion exitQuaternion;
        // exitQuaternion.eulerAngles = exitRotation;
        transform.eulerAngles = exitRotation;
        yield return new WaitForSeconds(3f);
        transform.tag = "Car";
        // Destroy(gameObject);
    }
}
