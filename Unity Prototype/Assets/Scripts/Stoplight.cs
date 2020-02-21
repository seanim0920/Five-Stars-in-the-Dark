using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoplight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayRepeating());
    }

    IEnumerator PlayRepeating()
    {
        while (true)
        {
            tag = "Stop";
            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(2).GetComponent<AudioSource>().Stop();

            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(8f);
            tag = "Go";
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<AudioSource>().Stop();

            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(1).GetComponent<AudioSource>().Play();

            yield return new WaitForSeconds(5f);
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<AudioSource>().Stop();

            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(2).GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(4f);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
