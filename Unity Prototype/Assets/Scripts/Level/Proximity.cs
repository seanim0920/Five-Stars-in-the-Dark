using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Proximity : MonoBehaviour
{
    private float eyesight = 4f;
    public AudioSource noise;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, transform.up * eyesight, Color.green);
        //print(hit.collider);

        if (Input.GetKeyDown("p"))
        {
            //switch proximity modes
            //noise.clip = "";
        }
    }

    void onTriggerStay2D(Collider2D col)
    {
        Vector3 difference = (col.gameObject.transform.position - transform.position);
        float distance = difference.magnitude;
        noise.volume = Mathf.Pow(((-distance / transform.localScale.y) + 1), 2);
        noise.panStereo = Vector3.Cross(difference, transform.up).magnitude * (Vector3.Dot(col.gameObject.transform.position - transform.position, transform.right) > 0 ? 1 : -1) / (transform.localScale.x/2);
        noise.pitch = (1 / eyesight) * Mathf.Pow(distance - eyesight, 2) + 1;
    }
}
