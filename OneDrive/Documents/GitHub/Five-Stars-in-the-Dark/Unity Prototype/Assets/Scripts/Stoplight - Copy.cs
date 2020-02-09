using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoplightOld : MonoBehaviour
{
    AudioSource audioData;
    public AudioClip red;
    public AudioClip green;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine("PlayRepeating");
    }

    IEnumerator PlayRepeating()
    {
        // Start function WaitAndPrint as a coroutine
        while (true)
        {
            sprite.color = new Color(1f, 0f, 0f, 0.5f);
            audioData.clip = red;
            audioData.Play();
            yield return new WaitForSeconds(10f);
            sprite.color = new Color(0f, 1f, 0f, 0.5f);
            audioData.clip = green;
            audioData.Play();
            yield return new WaitForSeconds(10f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
