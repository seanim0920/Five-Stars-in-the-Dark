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
        tag = "Stop";
        transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;

        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        for (int loop = 0; loop < loopAmount; loop++)
        {
            for (int cars = 0; cars < carsAmount; cars++)
            {
                GameObject npc = Instantiate(Resources.Load<GameObject>("FastCar"), transform);
                npc.GetComponent<Rigidbody2D>().isKinematic = true;
                npc.GetComponent<NPCMovement>().neutralSpeed = 2;
                npc.transform.localPosition = transform.GetChild(0).localPosition;
                npc.transform.Rotate(0, 0, -90);
                Destroy(npc, 4);
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(3f);
        }
        yield return new WaitForSeconds(8f);
        tag = "Go";
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
