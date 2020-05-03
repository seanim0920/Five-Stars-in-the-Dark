using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject soundSource;
    private AudioSource sound;
    private float shakeStore;

    private void Start()
    {
       sound = soundSource.GetComponent(typeof(AudioSource)) as AudioSource;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(isPaused)
            {
                resumeGame();
            } else
            {
                pauseGame();
            }
        }
    }

    public void resumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        sound.UnPause();
        SpeedShake.shakeOffset = shakeStore;
    }

    public void pauseGame()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        sound.Pause();
        shakeStore = SpeedShake.shakeOffset;
        SpeedShake.shakeOffset = 0;
    }

    public void toMenu()
    {
        resumeGame();
        LoadScene.Loader("Menu");
    }
    
    public void restartLevel()
    {
        resumeGame();
        LoadScene.Loader(SceneManager.GetActiveScene().name);
    }
}
