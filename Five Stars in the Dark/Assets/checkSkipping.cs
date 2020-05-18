using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkSkipping : MonoBehaviour
{
    public Text icon;
    // Update is called once per frame
    private void Start()
    {
    }
    void Update()
    {
        icon.enabled = SkipCutscenes.isSkipping;
        OverlayStatic.overlaid = SkipCutscenes.isSkipping;
    }
}
