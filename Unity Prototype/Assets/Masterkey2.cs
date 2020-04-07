using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Masterkey2 : MonoBehaviour
{
    public Button retry;
	public Button level;
	public Button menu;
	public static int flag = -1;
	
    // Start is called before the first frame update
     void Start()
    {
		Button e = retry.GetComponent<Button>();
		Button l = level.GetComponent<Button>();
		Button lb = menu.GetComponent<Button>();
		
		e.onClick.AddListener(TaskRetry);
		l.onClick.AddListener(TaskNextLvl);
		lb.onClick.AddListener(TaskMenu);
    }
	
	void TaskRetry(){
		flag = 1;
	}
	
	void TaskNextLvl(){
		flag = 2;
	}
	
	void TaskMenu(){
		flag = 3;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
