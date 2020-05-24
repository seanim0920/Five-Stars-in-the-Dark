using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimOnce : MonoBehaviour
{
    Animator[] animations;

    // Start is called before the first frame update
    void Start()
    {
        animations = transform.GetComponentsInChildren<Animator>();
        if (Masterkey.played)
        {
            foreach (Animator anim in animations)
            {
                anim.enabled = false;
            }
        } else
        {
            Masterkey.played = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
