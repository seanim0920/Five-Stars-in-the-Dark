using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosswalk : MonoBehaviour
{
    AudioSource audioData;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        audioData = transform.GetChild(0).GetComponent<AudioSource>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        StartCoroutine(PlayRepeating());
    }

    IEnumerator PlayRepeating()
    {
        // Start function WaitAndPrint as a coroutine
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5.0f, 12.0f));
            tag = "Stop";
            sprite.color = new Color(1f, 0f, 0f, 0.5f);
            audioData.Play();
            yield return new WaitForSeconds(5f);
            tag = "Go";
            sprite.color = new Color(0f, 1f, 0f, 0.5f);
            audioData.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
