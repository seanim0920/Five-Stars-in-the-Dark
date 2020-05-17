using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkSkipping : MonoBehaviour
{
    public Text icon;
    public Image noise;
    public Sprite[] noiseImages;
    private int i = 0;
    // Update is called once per frame
    private void Start()
    {
    }
    void Update()
    {
        icon.enabled = SkipCutscenes.isSkipping;
        noise.enabled = SkipCutscenes.isSkipping;
        if (SkipCutscenes.isSkipping)
        {
            i = (i + Random.Range(0, 4)) % 12;
            noise.sprite = noiseImages[i/3];
        }
    }
}
