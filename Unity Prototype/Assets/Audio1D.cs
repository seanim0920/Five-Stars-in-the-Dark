using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio1D : MonoBehaviour
{
    private AudioSource soundObj;
    private Camera viewport;
    // Start is called before the first frame update

    void Start()
    {
        viewport = Camera.main;
        soundObj = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<SpriteRenderer>().isVisible) { soundObj.volume = 0; return; }
        Vector3 viewPos = viewport.WorldToViewportPoint(transform.position);
        float xpos = (viewPos.x * 2f) - 1f;
        //print(viewPos.x);
        //ranges from -1 (left of screen) to +1 (right of screen)
        //we will also want to add depth in the future (how long the sound lasts before leaving to the left or right)
        float ypos = viewPos.y;
        //ranges from 1 (top of screen) to 0 (bottom of screen)

        soundObj.panStereo = 1.5f*xpos*(1f-Mathf.Pow((ypos),4));
        if (ypos <= 0.25f)
        {
            soundObj.volume = (-(Mathf.Pow(4*ypos - 1, 4))) + 1;
        }
        else
        {
            soundObj.volume = -1.75f*(Mathf.Pow(ypos - 0.13f, 4)) + 1;
        }
        soundObj.volume += (-0.01f*Mathf.Pow(xpos, 20f));
        //soundObj.volume = (-Mathf.Pow(xpos, 8f) + 0.3f*(Mathf.Pow(Mathf.Abs(ypos-1) - (ypos-1), 2)));
        //if you wanted fadeout at the sides
    }
}