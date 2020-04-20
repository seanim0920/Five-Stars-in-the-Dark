using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Masterkey : MonoBehaviour
{
	public Button start;
	public Button level;
	public Button levelBack;
	public GameObject levelOne, play, play2;
	public static bool egg = false;
	public static bool lvl = false;
    // Start is called before the first frame update
     void Start()
    {
		Button e = start.GetComponent<Button>();
		Button l = level.GetComponent<Button>();
		Button lb = levelBack.GetComponent<Button>();
		e.onClick.AddListener(TaskStart);
		l.onClick.AddListener(TaskLvl);
		lb.onClick.AddListener(TaskLvlReset);
    }
	
	void TaskStart(){
		egg = true;
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(play2);
	}
	
	void TaskLvl(){
		lvl = true;
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(levelOne);
	}
	
	void TaskLvlReset(){
		lvl = false;
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(play);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
