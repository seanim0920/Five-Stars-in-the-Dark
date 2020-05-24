using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionHandler : MonoBehaviour
{
    private string resolution = "low";
    public GameObject highButton;
    public GameObject medButton;
    public GameObject lowButton;
    public GameObject fullText;
    public GameObject windowText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void toggleFullscreen()
    {
        Screen.fullScreen = !(Screen.fullScreen);
        if(Screen.fullScreen)
        {
            windowText.SetActive(false);
            fullText.SetActive(true);
        }
        else
        {
            windowText.SetActive(true);
            fullText.SetActive(false);
        }
    }

    public void cycleResolution()
    {
        if(resolution == "high")
        {
            Screen.SetResolution(1366, 768, Screen.fullScreen);
            resolution = "med";
            medButton.SetActive(true);
            highButton.SetActive(false);
        } else if (resolution == "med")
        {
            Screen.SetResolution(1024, 576, Screen.fullScreen);
            resolution = "low";
            lowButton.SetActive(true);
            medButton.SetActive(false);
        } else
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
            resolution = "high";
            highButton.SetActive(true);
            lowButton.SetActive(false);
        }
    }
}
