using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio1DWide : MonoBehaviour
{
    private AudioSource leftEdge;
    private AudioSource rightEdge;
    private Camera viewport;
    public float width = 1;
    // Start is called before the first frame update

    void Start()
    {
        viewport = Camera.main;
        AudioSource baseSound = GetComponent<AudioSource>();
        //we duplicate the audiosource here to create a sound for the left and right edge of an object
        leftEdge = baseSound;
        rightEdge = gameObject.AddComponent<AudioSource>();
        rightEdge.loop = true;
        rightEdge.clip = leftEdge.clip;
        rightEdge.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<SpriteRenderer>().isVisible) { return; }
        Vector3 viewPos = viewport.WorldToViewportPoint(transform.position);
        float xpos = (viewPos.x * 2f) - 1f;
        print(viewPos.x);
        //ranges from -1 (left of screen) to +1 (right of screen)
        float width = -Mathf.Pow((0.36f*viewPos.y),2)+ Mathf.Pow((0.36f), 2);
        //eventually we want this to be parametrized so 0.5 and 0.6 change depending on the object's width
        //we will also want to add depth in the future (how long the sound lasts before leaving to the left or right)
        float leftxpos = xpos - width;
        float rightxpos = xpos + width;
        float ypos = (viewPos.y - 0.5f);

        leftEdge.panStereo = -Mathf.Pow((Mathf.Sqrt(leftxpos) * ypos),2) + leftxpos;
        rightEdge.panStereo = -Mathf.Pow((Mathf.Sqrt(leftxpos) * ypos), 2) + leftxpos;
        rightEdge.panStereo = rightxpos;
        leftEdge.volume = ( (-Mathf.Pow(leftxpos, 6f) + 1f) + -(Mathf.Pow( Mathf.Abs(ypos)-ypos,4 )) )/2;
        rightEdge.volume = ((-Mathf.Pow(rightxpos, 6f) + 1f) + -(Mathf.Pow(Mathf.Abs(ypos) - ypos, 4))) / 2;
    }
}