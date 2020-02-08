using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoplight : MonoBehaviour
{
    private AudioSource audioData;
    private GameObject NPC;

    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        StartCoroutine(PlayRepeating());
        //StartCoroutine("SpawnCar");
    }

    IEnumerator PlayRepeating()
    {
        // Start function WaitAndPrint as a coroutine
        while (true)
        {
            tag = "Stop";
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<AudioSource>().Stop();

            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(2).GetComponent<AudioSource>().Play();
            print("STOP NOW");
            yield return new WaitForSeconds(7f);
            print("GO NOW");
            tag = "Go";
            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(2).GetComponent<AudioSource>().Stop();

            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).GetComponent<AudioSource>().Play();

            yield return new WaitForSeconds(5f);
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<AudioSource>().Stop();

            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(1).GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(4f);
        }
    }
    //IEnumerator SpawnCar()
    //{
    //    // Start function WaitAndPrint as a coroutine
    //    while (true)
    //    {
    //        GameObject car = Instantiate(NPC, transform.position + transform.up * 2, Quaternion.Euler(0, 0, 90));
    //        car.GetComponent<Movement1D>().setSpeed(Random.Range(0.1f, 0.2f));
    //        Destroy(car, 1.5f);
    //        yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
    //    }
    //}

    // Update is called once per frame
    void Update()
    {

    }
}
