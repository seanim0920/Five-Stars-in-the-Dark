using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkPaused : MonoBehaviour
{
    private Text[] icons;
    // Start is called before the first frame update
    void Start()
    {
        icons = GetComponentsInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        OverlayStatic.overlaid = PauseMenu.isPaused;
    }
}
