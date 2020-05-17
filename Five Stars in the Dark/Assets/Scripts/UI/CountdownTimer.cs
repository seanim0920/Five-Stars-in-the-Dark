using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public static float levelCompleteTime { get; set; }
    public static float currentTime { get; set; }

    public static bool isTracking = false;
    // Start is called before the first frame update
    void Start()
    {
        // waitTime = 100.0f;
        levelCompleteTime = 600.0f;
        currentTime = levelCompleteTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTracking)
        {
            currentTime -= 1 * Time.deltaTime;
        }
    }

    //because currentTime is static, it needs a method to access
    public static float getCurrentTime()
    {
        return currentTime;
    }
    public static void setTracking(bool enabled)
    {
        isTracking = enabled;
    }
}
