using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckEngineDisplay : MonoBehaviour
{
    Image image;
    private Color SmallColor;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        SmallColor = new Color(1, 1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (5 - TrackErrors.getErrors() <= 1)
        {
            if (!image.enabled)
            {
                GetComponent<AudioSource>().Play();
            }
            SmallColor.a = 1 - Blur.getAmount();
            image.color = SmallColor;
            image.enabled = true;
        } else
        {
            image.enabled = false;
        }
    }
}
