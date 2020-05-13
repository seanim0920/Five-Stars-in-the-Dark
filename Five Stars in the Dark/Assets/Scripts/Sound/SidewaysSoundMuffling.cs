using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SidewaysSoundMuffling : MonoBehaviour
{
    public AudioMixer leftSource;
    public AudioMixer rightSource;
    private float maxFrequency = 20000;
    private float minFrequency = 400;
    private float hearingDistance = 20f;
    private float minDistance = 10f;
    private float sharpness = 10;
    private float maxVolume = 20;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //set neutral parameters
        rightSource.SetFloat("LowF", maxFrequency);
        rightSource.SetFloat("Volume", 0);
        leftSource.SetFloat("LowF", maxFrequency);
        leftSource.SetFloat("Volume", 0);

        RaycastHit2D leftEar = Physics2D.Raycast(transform.position, -transform.right, hearingDistance);
        RaycastHit2D rightEar = Physics2D.Raycast(transform.position, transform.right, hearingDistance);
        if (leftEar.collider && leftEar.collider.gameObject.tag != "Zone")
        {
            float distance = (leftEar.point - (Vector2)transform.position).magnitude;
            //print(distance);
            leftSource.SetFloat("LowF", (minFrequency + (((maxFrequency - minFrequency) / (Mathf.Pow((hearingDistance - minDistance), sharpness))) * Mathf.Pow((distance - minDistance), sharpness))));
            leftSource.SetFloat("Volume", maxVolume + (-maxVolume / Mathf.Pow(hearingDistance, 2)) * Mathf.Pow(distance, 2));
            //rightSource.SetFloat("HighF", (minFrequency*3) + ((-minFrequency*3) / Mathf.Pow(hearingDistance, 2)) * Mathf.Pow(distance, 2));
        }
        if (rightEar.collider && rightEar.collider.gameObject.tag != "Zone")
        {
            float distance = (rightEar.point - (Vector2)transform.position).magnitude;
            //print(distance);
            rightSource.SetFloat("LowF", (minFrequency + (((maxFrequency - minFrequency) / (Mathf.Pow((hearingDistance - minDistance), sharpness))) * Mathf.Pow((distance - minDistance), sharpness))));
            rightSource.SetFloat("Volume", maxVolume + (-maxVolume / Mathf.Pow(hearingDistance, 2)) * Mathf.Pow(distance, 2));
            //leftSource.SetFloat("HighF", (minFrequency * 3) + ((-minFrequency * 3) / Mathf.Pow(hearingDistance, 2)) * Mathf.Pow(distance, 2));
        }
    }
}
