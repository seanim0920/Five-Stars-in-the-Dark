using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalSound : MonoBehaviour
{
    AudioListener listener;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        listener = (AudioListener)FindObjectOfType(typeof(AudioListener));
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = listener.gameObject.transform.position - transform.position;
        if (difference.magnitude <= audioSource.maxDistance)
        {
            Vector3 directionToCar = difference.normalized;
            Vector3 direction = (this.gameObject.transform.GetChild(0).transform.position - transform.position).normalized;

            float newVolume = 4 * Vector3.Dot(direction, directionToCar) - 3;
            if (difference.magnitude <= audioSource.minDistance - 2f)
            {
               newVolume -= (-difference.magnitude / (audioSource.minDistance - 2f)) + 1.5f;
            }
            audioSource.volume = newVolume;
        }
    }
}
