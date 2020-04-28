using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Masterkey2 : MonoBehaviour
{
    public Button retry;
	public Button menu;
    public static string sceneName = "Level 1";
	
    // Start is called before the first frame update
     void Start()
    {
		Button e = retry.GetComponent<Button>();
		Button lb = menu.GetComponent<Button>();
		
		e.onClick.AddListener(() => LoadScene.Loader(sceneName));
		lb.onClick.AddListener(() => LoadScene.Loader("Menu"));
    }

    // Update is called once per frame
    void Update()
    {
    }

    private string getPrev()
    {
        int prevIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("This scene index: " + prevIndex);
        --prevIndex;
        Debug.Log("Previous scene index: " + prevIndex);
        string prevName = SceneManager.GetSceneByBuildIndex(prevIndex).name;
        Debug.Log("Previous scene name: " + prevName);
        return prevName;
    }
}
