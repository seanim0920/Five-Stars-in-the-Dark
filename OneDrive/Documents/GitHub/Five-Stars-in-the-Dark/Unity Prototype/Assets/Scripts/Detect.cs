using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{
    float rayLength = 1.5f;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, rayLength, 1 << 8);
        //Debug.Log(hit.collider);
        if (hit.collider != null)
        {
            float distance = Vector2.Distance(hit.point, new Vector2(transform.position.x, transform.position.y));
            audioSource.volume = Mathf.Pow((rayLength - distance) / rayLength, 2f);
        } else
        {
            audioSource.volume = 0;
        }
    }
}
