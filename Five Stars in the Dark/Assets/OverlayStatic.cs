using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayStatic : MonoBehaviour
{
    public Image noise;
    public Sprite[] noiseImages;
    private int i = 0;
    public static bool overlaid = false;
    // Start is called before the first frame update
    void Start()
    {
        overlaid = false;
    }

    // Update is called once per frame
    void Update()
    {
        noise.enabled = overlaid;
        if (overlaid)
        {
            i = (i + Random.Range(0, 4)) % 12;
            noise.sprite = noiseImages[i / 3];
        }
    }
}
