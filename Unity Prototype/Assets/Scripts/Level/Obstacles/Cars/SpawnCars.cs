using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    public int cars;
    GameObject NPC;
    Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        NPC = Resources.Load<GameObject>("NPC");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerTransform = col.gameObject.transform;
            StartCoroutine(SpawnCar());
        }
    }
    IEnumerator SpawnCar()
    {
        for (int i = 0; i < cars; i++)
        {
            GameObject car = Instantiate(NPC, new Vector3(transform.position.x + Random.Range(-3,3), playerTransform.position.y + 6, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 6.0f));
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            if (col.gameObject.transform.position.x > playerTransform.position.x)
            {
                col.gameObject.transform.Rotate(0, 0, -90);
            }
            else if (col.gameObject.transform.position.x <= playerTransform.position.x)
            {
                col.gameObject.transform.Rotate(0, 0, 90);
            }

            Destroy(col.gameObject, 2);
        }
    }
}