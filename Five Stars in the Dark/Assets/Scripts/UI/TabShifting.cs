using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabShifting : MonoBehaviour
{
	//1 Volume, 2 Resolution, 3 Controls
	public int flag = 1;
	
	public Button Volume;
	public Button Resolution;
	public Button Controls;
	public GameObject v;
	public GameObject r;
	public GameObject c;
    // Start is called before the first frame update
    void Start()
    {
        Volume.onClick.AddListener(TaskVol);
		Resolution.onClick.AddListener(TaskRes);
		Controls.onClick.AddListener(TaskCon);
		flag = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(flag == 1) {
			v.SetActive(true);
			r.SetActive(false);
			c.SetActive(false);
		}
		else if (flag == 2) {
			v.SetActive(false);
			r.SetActive(true);
			c.SetActive(false);
		}
 	    else if (flag == 3) {
			v.SetActive(false);
			r.SetActive(false);
			c.SetActive(true);
	    }
    }
	
	void TaskVol() {
		flag = 1;
	}
	
	void TaskRes() {
		flag = 2;
	}
	
	void TaskCon() {
		flag = 3;
	}
	
}
