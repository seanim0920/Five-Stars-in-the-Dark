using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCarBehindPlayer : MonoBehaviour
{
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
            GameObject car = Instantiate(NPC, playerTransform.position - new Vector3(0, 6, 0), Quaternion.identity);
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
