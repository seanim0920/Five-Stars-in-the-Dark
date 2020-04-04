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
	}
	
	void TaskLvl(){
		lvl = true;
	}
	
	void TaskLvlReset(){
		lvl = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
