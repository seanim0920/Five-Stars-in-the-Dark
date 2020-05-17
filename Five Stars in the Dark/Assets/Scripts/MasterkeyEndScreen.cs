using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MasterkeyEndScreen : MonoBehaviour
{
    public Button retry;
    public Button next;
    public Button menu;
    public static string currentLevel = "Level 2";

    // Start is called before the first frame update
    void Start()
    {
        retry.onClick.AddListener(() => LoadScene.Loader(currentLevel));
        next.onClick.AddListener(() => LoadScene.Loader(getNextSceneName(currentLevel)));
        menu.onClick.AddListener(() => LoadScene.Loader("Menu"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private string getNextSceneName(string currentLevel)
    {
        int currIndex = SceneManager.GetSceneByName(currentLevel).buildIndex;
        ++currIndex;
        string nextName = SceneManager.GetSceneByBuildIndex(currIndex).name;
        return nextName;
    }
}
