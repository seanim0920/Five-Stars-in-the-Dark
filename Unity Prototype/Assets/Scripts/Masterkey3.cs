using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Masterkey3 : MonoBehaviour
{
    public Button retry;
    public Button next;
    public Button menu;
    public static string sceneName = "Level 2";
    public static string prevName = "Level 1";

    // Start is called before the first frame update
    void Start()
    {
        Button e = retry.GetComponent<Button>();
        Button l = next.GetComponent<Button>();
        Button lb = menu.GetComponent<Button>();

        e.onClick.AddListener(() => LoadScene.Loader(prevName));
        l.onClick.AddListener(() => LoadScene.Loader(sceneName));
        lb.onClick.AddListener(() => LoadScene.Loader("Menu"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
