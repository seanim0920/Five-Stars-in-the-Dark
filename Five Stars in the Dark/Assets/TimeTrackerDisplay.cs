using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrackerDisplay : MonoBehaviour
{
    private static Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = "Time remaining: " + CountdownTimer.getCurrentTime().ToString("00.0");
    }
}
