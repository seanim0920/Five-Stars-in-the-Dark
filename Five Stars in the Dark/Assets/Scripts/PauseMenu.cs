using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    private float shakeStore;
    private bool dialoguePaused = false;
    private Button resumeButton;
    AudioSource[] sources;

    private void Start()
    {
        sources = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        resumeButton = GetComponentInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
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
        sources = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
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

        EventSystem.current.SetSelectedGameObject(null);
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

        EventSystem.current.SetSelectedGameObject(null);
        resumeButton = GetComponentInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
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
