using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    Collider2D curb;
    public Vector2 velocity;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        curb = GameObject.Find("Tilemap_Curb").GetComponent<Collider2D>();
        startPos = transform.position;
        while (curb.OverlapPoint(startPos))
        {
            startPos = startPos - new Vector3(velocity.x, velocity.y, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(velocity.x, velocity.y, 0);
        if (!curb.OverlapPoint(transform.position))
        {
            velocity *= -1;
            //transform.position = startPos;
        }
    }

    /*
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log(" pedestrian left curb ");
        if (other.gameObject.tag == "Curb")
        {
        }
    }*/
}
