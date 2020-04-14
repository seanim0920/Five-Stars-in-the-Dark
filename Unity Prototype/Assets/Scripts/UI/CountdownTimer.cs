using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    private static Text timerText;
    private static float waitTime;

    public static float levelCompleteTime { get; set; }
    public static float currentTime { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        // waitTime = 100.0f;
        levelCompleteTime = 600.0f;
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        currentTime = levelCompleteTime;
    }

    // Update is called once per frame
    void Update()
    {
        updateDisplay();
        // if(waitTime > 0.0f)
        // {
        //     waitTime -= 1 * Time.deltaTime;
        // }
        // else
        // {
            currentTime -= 1 * Time.deltaTime;
            updateDisplay();
            if (currentTime <= 0)
            {
                SceneManager.LoadScene("EndScreen", LoadSceneMode.Single);
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            }
        // }
    }

    private static void updateDisplay()
    {
        timerText.text = "Time remaining: " + currentTime.ToString("00.0");
    }
}
