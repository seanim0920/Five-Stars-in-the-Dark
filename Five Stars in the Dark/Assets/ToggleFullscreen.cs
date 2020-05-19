using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFullscreen : MonoBehaviour
{
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
}
