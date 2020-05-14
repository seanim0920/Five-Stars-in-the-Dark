using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    private float shakeStore;
    private bool dialoguePaused = false;
    AudioSource[] sources;

    private void Start()
    {
        sources = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || (Gamepad.current != null && Gamepad.current.startButton.isPressed))
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
        foreach (AudioSource source in sources)
        {
            if (!source.isPlaying && source.time != 0)
            {
                if (source.gameObject.CompareTag("Constructor"))
                {
                    if (dialoguePaused)
                        dialoguePaused = false;
                    else
                        continue;
                }
                source.UnPause();
            }
        }
        MovementShake.shakeOffset = shakeStore;
    }

    public void pauseGame()
    {
        sources = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        foreach (AudioSource source in sources)
        {
            if (source.isPlaying)
            {
                if (source.gameObject.CompareTag("Constructor"))
                    dialoguePaused = true;
                source.Pause();
            }
        }
        shakeStore = MovementShake.shakeOffset;
        MovementShake.shakeOffset = 0;
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
