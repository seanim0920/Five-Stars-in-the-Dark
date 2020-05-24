using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Masterkey : MonoBehaviour
{
    public Button start;
    public Button play;
    public Button instructionsBack;
    public Button level;
    public Button levelBack;
    public Button settings;
    public Button settingsBack;
    public static bool egg = false;
    public static bool lvl = false;
    public static string sceneName = "Level 1";
    public Button level1;
    public Button level2;
    public Button level3;
    public Button level4;
    public Slider bgmSlider;

    public static bool played = false;

    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(TaskStart);
        level.onClick.AddListener(TaskLvl);
        levelBack.onClick.AddListener(TaskTitle);
        instructionsBack.onClick.AddListener(TaskTitle);
        settings.onClick.AddListener(TaskSettings);
        settingsBack.onClick.AddListener(TaskTitle);

        level1.onClick.AddListener(() => sceneName = "Level 1");
        level2.onClick.AddListener(() => sceneName = "Level 2");
        level3.onClick.AddListener(() => sceneName = "Level 3");
        level4.onClick.AddListener(() => sceneName = "Level 4");
    }

    // Update is called once per frame
    void Update()
    {
    }

    void TaskStart()
    {
        egg = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(play.gameObject);
    }

    void TaskLvl()
    {
        lvl = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(level1.gameObject);
    }

    public void TaskTitle()
    {
        egg = false;
        lvl = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(start.gameObject);
    }

    public void TaskSettings()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(bgmSlider.gameObject);
    }
}
