using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnCar : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player passed this car by x amount (probably the logarithmic rolloff)
        if(Mathf.Abs(player.transform.position.y - transform.position.y) > transform.GetChild(0).GetComponent<AudioSource>().maxDistance)
        {
            // If so, Destroy the car
            Destroy(gameObject);
        }
    }
}
