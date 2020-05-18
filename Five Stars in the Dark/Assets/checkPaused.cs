using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkPaused : MonoBehaviour
{
    private Text[] icons;
    private bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        icons = GetComponentsInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused != isPaused)
        {
            OverlayStatic.overlaid = PauseMenu.isPaused;
        }
        isPaused = PauseMenu.isPaused;
    }
}
