using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    private static Text timerText;

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
        currentTime -= 1 * Time.deltaTime;
        updateDisplay();
        if (currentTime <= 0)
        {
            ScoreStorage.Instance.setScoreAll();
            MasterkeyFailScreen.currentLevel = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("FailScreen", LoadSceneMode.Single);
        }
    }

    private static void updateDisplay()
    {
        timerText.text = "Time remaining: " + currentTime.ToString("00.0");
    }

    //because currentTime is static, it needs a method to access
    public float getCurrentTime()
    {
        return currentTime;
    }
}
