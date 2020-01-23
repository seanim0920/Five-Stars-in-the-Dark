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
        audioData = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine("PlayRepeating");
    }

    IEnumerator PlayRepeating()
    {
        // Start function WaitAndPrint as a coroutine
        while (true)
        {
            yield return new WaitForSeconds(5f);
            sprite.color = new Color(1f, 0f, 0f, 0.5f);
            //Debug.Log("new color is " + sprite.color);
            audioData.Play();
            yield return new WaitForSeconds(audioData.clip.length);
            sprite.color = new Color(0f, 1f, 0f, 0.5f);
            yield return new WaitForSeconds(10f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
