using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Masterkey3 : MonoBehaviour
{
    public Button retry;
    public Button next;
    public Button menu;
    public static string sceneName = "Level 1";

    // Start is called before the first frame update
    void Start()
    {
        Button e = retry.GetComponent<Button>();
        Button l = next.GetComponent<Button>();
        Button lb = menu.GetComponent<Button>();

        e.onClick.AddListener(() => LoadScene.Loader(sceneName));
        l.onClick.AddListener(() => LoadScene.Loader(getNext()));
        lb.onClick.AddListener(() => LoadScene.Loader("Menu"));
    }

    // Update is called once per frame
    void Update()
    {
    }

    private string getNext()
    {
        string prevName = sceneName;
        if(prevName == "Level 1") {
            return "Level 2";
        } else if (prevName == "Level 2") {
            return "Level 3";
        } else if (prevName == "Level 2") {
            return "Level 3";
        } else if (prevName == "Level 3") {
            return "Level 4";
        } else if (prevName == "Level 4") {
            return "Level 5";
        } else {
            return "Menu";
        }
    }
}
