using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Masterkey : MonoBehaviour
{
    public Button start;
    public Button play;
    public Button level;
    public Button levelBack;
    public static int flag = 0;
    public static string sceneName = "Level 1";
    public Button level1;
    public Button level2;
    public Button level3;
    public Button level4;
	
	public Button Instruc_Back;
	public Button Settings;
	public Button Set_Back;

	public static bool played = false;

    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(TaskStart);
        level.onClick.AddListener(TaskLvl);
        levelBack.onClick.AddListener(TaskTitle);

        Instruc_Back.onClick.AddListener(TaskTitle);
		Settings.onClick.AddListener(TaskSettings);
		Set_Back.onClick.AddListener(TaskTitle);

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
        flag = 1;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(play.gameObject);
    }

    void TaskLvl()
    {
        flag = 2;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(level1.gameObject);
    }

    public void TaskTitle()
    {
        flag = 0;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(start.gameObject);
    }
	
	public void TaskSettings()
    {
        flag = 3;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(start.gameObject);
    }
}
