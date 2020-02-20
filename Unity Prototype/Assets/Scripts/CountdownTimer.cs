using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    private static Text timerText;
    private static float levelCompleteTime = 360f;

    public static float currentTime { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<Text>();
        currentTime = levelCompleteTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        updateDisplay();
        if (currentTime <= 0)
        {
            SceneManager.LoadScene("EndScreen", LoadSceneMode.Single);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }

    private static void updateDisplay()
    {
        timerText.text = "Time remaining: " + currentTime.ToString("00.0");
    }
}
