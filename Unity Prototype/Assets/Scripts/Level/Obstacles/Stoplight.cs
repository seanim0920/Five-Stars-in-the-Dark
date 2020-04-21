using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoplight : MonoBehaviour
{
    public int loopAmount;
    public int carsAmount;
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
            for (int loop = 0; loop < loopAmount; loop++)
            {
                for (int cars = 0; cars < carsAmount; cars++)
                {
                    GameObject npc = Instantiate();
                    npc.transform.localRotation = ;
                }
            }
            yield return new WaitForSeconds(8f);
            tag = "Go";
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<AudioSource>().Stop();

            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(1).GetComponent<AudioSource>().Play();

            yield return new WaitForSeconds(5f);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
