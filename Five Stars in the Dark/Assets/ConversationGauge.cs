using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationGauge : MonoBehaviour
{
    [SerializeField]
    private float gauge = 100; //when it hits 0 the rest of the conversation plays and the car speeds off
    public AudioSource noise;
    public AudioSource convo;
    public AudioSource ding;
    public AudioSource rev;
    private bool destroyed = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gauge += 0.01f;
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
            gauge -= noise.pitch * 10 * Time.deltaTime;
            noise.volume = gauge/100;
            convo.panStereo = noise.panStereo;
            convo.volume = 1 - gauge/100;
            convo.pitch = noise.pitch;
            if (gauge <= 0 && !destroyed)
            {
                destroyed = true;
                transform.parent.tag = "Car";
                StartCoroutine(driveAway());
            }
        }
    }

    IEnumerator driveAway()
    {
        ding.Play();
        yield return new WaitForSeconds(3);
        rev.Play();
        for (int i = 0; i < 100; i++)
        {
            transform.parent.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 300));
            yield return new WaitForFixedUpdate();
        }
        Destroy(this);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //should be adjusted to detect the closest car to the player, if there are multiple cars in the zone
        if (col.gameObject.tag == "Player")
        {
            convo.volume = 0;
            noise.volume = 0;
        }
    }
}
