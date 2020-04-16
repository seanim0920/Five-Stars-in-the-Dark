using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Masterkey : MonoBehaviour
{
	public Button start;
	public Button level;
	public Button levelBack;
	public static bool egg = false;
	public static bool lvl = false; 
    public static string sceneName;
    
    // Start is called before the first frame update
    void Start()
    {
		Button e = start.GetComponent<Button>();
		Button l = level.GetComponent<Button>();
		Button lb = levelBack.GetComponent<Button>();
		e.onClick.AddListener(TaskStart);
		l.onClick.AddListener(TaskLvl);
		lb.onClick.AddListener(TaskLvlReset);
        setLVL1();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void TaskStart() {
		egg = true;
	}
    void TaskLvl() {
		lvl = true;
	}
	void TaskLvlReset() {
		lvl = false;
	}
    public static void setLVL1() {
        sceneName = "Level 1";
    }
    public static void setLVL2() {
        sceneName = "Level 2";
    }
    public static void setLVL3() {
        sceneName = "Level 3";
    }
    public static void setLVL4() {
        sceneName = "Level 4";
    }
}
