using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public AudioSource pauseStartSound;
    public AudioSource pauseMenuSound;
    public AudioSource pauseEndSound;
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    private float shakeStore;
    private Button resumeButton;

    private void Start()
    {
        isPaused = false;
        resumeButton = GetComponentInChildren<Button>();
        pauseStartSound.ignoreListenerPause = true;
        pauseMenuSound.ignoreListenerPause = true;
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
        OverlayStatic.overlaid = false;
        pauseMenuSound.Stop();
        pauseEndSound.Play();
        AudioListener.pause = false;
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        MovementShake.shakeOffset = shakeStore;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void pauseGame()
    {
        if (SkipCutscenes.isSkipping) return;
        OverlayStatic.overlaid = true;
        pauseMenuSound.Play();
        pauseStartSound.Play();
        AudioListener.pause = true;
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        shakeStore = MovementShake.shakeOffset;
        MovementShake.shakeOffset = 0;

        EventSystem.current.SetSelectedGameObject(null);
        resumeButton = GetComponentInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        GameObject settings = GameObject.Find("Settings Panel");
        if(settings != null)
        {
            settings.SetActive(false);
        }
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

    public void goToSettings()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("BGM"));
    }

    public void goBackToPause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("SettingsButton"));
    }
}
