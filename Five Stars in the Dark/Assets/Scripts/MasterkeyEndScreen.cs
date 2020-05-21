using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterkeyEndScreen : MonoBehaviour
{
    public Button retry;
    public Button next;
    public Button menu;
    public static int currentLevelBuildIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        retry.onClick.AddListener(() => LoadScene.Loader(currentLevelBuildIndex));
        next.onClick.AddListener(() => LoadScene.Loader(currentLevelBuildIndex+1));
        menu.onClick.AddListener(() => LoadScene.Loader("Menu"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
