using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationGauge : MonoBehaviour
{
    public float gauge = 100; //when it hits 0 the rest of the conversation plays and the car speeds off
    private AudioSource noise;
    // Start is called before the first frame update
    void Start()
    {
        noise = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D col)
    {
        //should be adjusted to detect the closest car to the player, if there are multiple cars in the zone
        if (col.gameObject.tag == "Player")
        {
            Vector3 difference = (col.gameObject.transform.position - transform.parent.transform.position);
            float distance = difference.magnitude;
            float eyesight = transform.localScale.y * transform.parent.transform.localScale.y;
            noise.volume = Mathf.Pow(((-distance / (eyesight)) + 1), 2) * 1.1f;
            //print("proximity volume is " + distance + " " + noise.volume);
            Vector3 posRelativeToPlayer = transform.parent.transform.InverseTransformPoint(col.gameObject.transform.position);
            noise.panStereo = - posRelativeToPlayer.x / (transform.localScale.x / 2);
            noise.pitch = noise.volume * 2 + 1;
            gauge -= noise.volume * 2 * Time.deltaTime;
            if (gauge <= 0)
            {
                Destroy(transform.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //should be adjusted to detect the closest car to the player, if there are multiple cars in the zone
        if (col.gameObject.tag == "Player")
        {
            noise.volume = 0;
        }
    }
}
