using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Proximity : MonoBehaviour
{
    private float eyesight = 2.5f;
    public AudioSource leftnoise;
    public AudioSource rightnoise;
    public AudioMixer radio;
    private float minNoise = -60;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D frontSightLeft = Physics2D.Raycast(transform.position - new Vector3(transform.localScale.x / 2, 0, 0), transform.up, eyesight);
        RaycastHit2D frontSightRight = Physics2D.Raycast(transform.position + new Vector3(transform.localScale.x / 2, 0, 0), transform.up, eyesight);
        //Debug.DrawRay(transform.position, transform.up * eyesight, Color.green);
        //print(hit.collider);
        leftnoise.volume = 0;
        radio.SetFloat("Volume", 0);
        if ((frontSightLeft.collider && frontSightLeft.collider.gameObject.tag == "Car") || (frontSightRight.collider && frontSightRight.collider.gameObject.tag == "Car"))
        {
            float distance = Mathf.Min((frontSightLeft.point - (Vector2)transform.position).magnitude, (frontSightRight.point - (Vector2)transform.position).magnitude);
            leftnoise.volume = 1 - (Mathf.Pow(distance/eyesight, 4));
            radio.SetFloat("Volume", Mathf.Min((-minNoise/Mathf.Pow(eyesight, 2))*Mathf.Pow(distance, 2) - 60,0));
            leftnoise.pitch = (1/eyesight)*Mathf.Pow(distance-eyesight, 2) + 1;
        }
        rightnoise.volume = leftnoise.volume;
        rightnoise.pitch = leftnoise.pitch;
    }
}
