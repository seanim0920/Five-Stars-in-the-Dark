using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPassDetection : MonoBehaviour
{
    public AudioSource leftSource;
    public AudioSource rightSource;
    private float maxFrequency = 20000;
    private float minFrequency = 400;
    private float hearingDistance = 2.5f;
    private float minDistance = 0.41f;
    private float sharpness = 6;
    private float maxVolume = 15;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D leftEar = Physics2D.Raycast(transform.position, -transform.right, hearingDistance);
        if (leftEar.collider && leftEar.collider.gameObject.tag != "Zone")
        {
            float distance = (leftEar.point - (Vector2)transform.position).magnitude;
            print(distance);
            leftSource.outputAudioMixerGroup.audioMixer.SetFloat("LowF", (minFrequency + (((maxFrequency-minFrequency)/(Mathf.Pow((hearingDistance - minDistance), sharpness)))*Mathf.Pow((distance-minDistance), sharpness))));
            leftSource.outputAudioMixerGroup.audioMixer.SetFloat("Volume", maxVolume+(-maxVolume/Mathf.Pow(hearingDistance, 2))*Mathf.Pow(distance, 2));
            rightSource.outputAudioMixerGroup.audioMixer.SetFloat("HighF", (minFrequency*3) + ((-minFrequency*3) / Mathf.Pow(hearingDistance, 2)) * Mathf.Pow(distance, 2));
        }
        RaycastHit2D rightEar = Physics2D.Raycast(transform.position, transform.right, hearingDistance);
        if (rightEar.collider && rightEar.collider.gameObject.tag != "Zone")
        {
            float distance = (rightEar.point - (Vector2)transform.position).magnitude;
            print(distance);
            rightSource.outputAudioMixerGroup.audioMixer.SetFloat("LowF", (minFrequency + (((maxFrequency - minFrequency) / (Mathf.Pow((hearingDistance - minDistance), sharpness))) * Mathf.Pow((distance - minDistance), sharpness))));
            rightSource.outputAudioMixerGroup.audioMixer.SetFloat("Volume", maxVolume + (-maxVolume / Mathf.Pow(hearingDistance, 2)) * Mathf.Pow(distance, 2));
            leftSource.outputAudioMixerGroup.audioMixer.SetFloat("HighF", (minFrequency * 3) + ((-minFrequency * 3) / Mathf.Pow(hearingDistance, 2)) * Mathf.Pow(distance, 2));
        }
    }
}
