using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionHandler : MonoBehaviour
{
    private string resolution = "high";
    public GameObject highButton;
    public GameObject medButton;
    public GameObject lowButton;
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
        Debug.Log("fullscreen is " + Screen.fullScreen);
        Screen.fullScreen = !(Screen.fullScreen);
        Debug.Log("fullscreen is now " + Screen.fullScreen);
    }

    public void cycleResolution()
    {
        if(resolution == "high")
        {
            Screen.SetResolution(1366, 768, Screen.fullScreen);
            resolution = "med";
            medButton.SetActive(false);
            lowButton.SetActive(true);
        } else if (resolution == "med")
        {
            Screen.SetResolution(1024, 576, Screen.fullScreen);
            resolution = "low";
            lowButton.SetActive(false);
            highButton.SetActive(true);
        } else
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
            resolution = "high";
            highButton.SetActive(false);
            medButton.SetActive(true);
        }
    }
}
