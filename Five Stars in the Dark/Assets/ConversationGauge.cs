using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationGauge : MonoBehaviour
{
    [SerializeField]
    private float gauge = 100; //when it hits 0 the rest of the conversation plays and the car speeds off
    public AudioSource noise;
    public AudioSource convo;
    // Start is called before the first frame update
    void Start()
    {
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
            noise.pitch = Mathf.Pow(((-distance / (eyesight)) + 1), 2) * 4f;
            Vector3 posRelativeToPlayer = transform.parent.transform.InverseTransformPoint(col.gameObject.transform.position);
            noise.panStereo = - posRelativeToPlayer.x / (transform.localScale.x / 2);
            gauge -= noise.pitch * 25 * Time.deltaTime;
            noise.volume = gauge/100;
            convo.volume = 1 - gauge / 100;
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
