using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public Rigidbody2D body;
    private AudioSource soundObj;
    private Camera viewport;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        soundObj = GetComponent<AudioSource>();
        viewport = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewPos = viewport.WorldToViewportPoint(body.transform.position);
        float xpos = (viewPos.x * 2f) - 1f;
        //print(viewPos.x);
        //ranges from -1 (left of screen) to +1 (right of screen)
        //we will also want to add depth in the future (how long the sound lasts before leaving to the left or right)
        float ypos = viewPos.y;
        //ranges from 1 (top of screen) to 0 (bottom of screen)

        soundObj.panStereo = -1.2f * xpos * (1f - Mathf.Pow((ypos), 4));
    }
}
