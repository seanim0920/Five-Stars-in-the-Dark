using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    public int activeCars = 3;
    GameObject NPC;
    // Start is called before the first frame update
    void Start()
    {
        NPC = Resources.Load<GameObject>("NPC");
        StartCoroutine(SpawnCar());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnCar()
    {
        for (int i = 0; i < activeCars; i++)
        {
            GameObject car = Instantiate(NPC, randomPosWithinZone(), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            col.gameObject.transform.position = randomPosWithinZone();
        }
    }

    Vector3 randomPosWithinZone()
    {
        return new Vector3(Random.Range(-3f, 3f), 0, 0) + transform.position - transform.up * transform.localScale.y / 2;
    }
}