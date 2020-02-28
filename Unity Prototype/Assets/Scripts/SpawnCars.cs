using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    public int maxActiveCars = 3;
    private int activeCars = 0;
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

    IEnumerator SpawnCarsAroundPlayer(Transform playerTransform)
    {
        while (activeCars < maxActiveCars)
        {
            GameObject car = Instantiate(NPC, randomPosWithinZone(playerTransform), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
            activeCars++;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerTransform = col.gameObject.transform;
            StartCoroutine(SpawnCarsAroundPlayer(col.gameObject.transform));
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
            activeCars--;
        }
    }

    Vector3 randomPosWithinZone(Transform playerTransform)
    {
        if (Random.Range(0, 2) == 0) return new Vector3(Random.Range(-3f, 3f), -6, 0) + playerTransform.position;
        else return new Vector3(Random.Range(-3f, 3f), 6, 0) + playerTransform.position;
    }
}