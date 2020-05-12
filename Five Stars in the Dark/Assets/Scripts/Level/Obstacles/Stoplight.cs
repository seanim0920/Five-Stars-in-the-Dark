using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoplight : MonoBehaviour
{
    public int loopAmount = 3;
    public string pattern = "w";
    private string objectDirectory = "Prefabs/Obstacles/";
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
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == 'w')
                {
                    GameObject npc = Instantiate(Resources.Load<GameObject>(objectDirectory + "StoplightCar"), transform);
                    npc.GetComponent<Rigidbody2D>().isKinematic = true;
                    npc.GetComponent<NPCMovement>().neutralSpeed = 2;
                    npc.transform.localPosition = transform.GetChild(0).localPosition;
                    npc.transform.Rotate(0, 0, -90);
                    Destroy(npc, 8);
                }
                yield return new WaitForSeconds(1f);
            }
        }
        tag = "Go";
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
