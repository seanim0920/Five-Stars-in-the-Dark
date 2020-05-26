using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabShifting : MonoBehaviour
{
	//1 Volume, 2 Resolution, 3 Controls
	public int flag = 1;
	public bool isSubtitle = true;
	
	public Button Main_Settings;
	
	public Button Volume;
	public Button Resolution;
	public Button Controls;
	
	public Button Sub;
	public Text onOff;
	
	public Button Graphics_Screen;
	public Button Graphics_Res;
	
	public GameObject v;
	public GameObject r;
	public GameObject c;
	
	public Slider m;
	public Slider s;
	public Slider d;
	public Slider rc;
	
	public Toggle Keyboard;
	public Toggle Gamepad;
	public Toggle Wheel;
	
	Image vr;
	Image rr;
	Image cr;
	
    // Start is called before the first frame update
    void Start()
    {
        Volume.onClick.AddListener(TaskVol);
		Resolution.onClick.AddListener(TaskRes);
		Controls.onClick.AddListener(TaskCon);
		
		Keyboard.onValueChanged.AddListener(delegate { TaskResetHighlight();});
		Gamepad.onValueChanged.AddListener(delegate { TaskResetHighlight();});
		Wheel.onValueChanged.AddListener(delegate { TaskResetHighlight();});
		
		Graphics_Screen.onClick.AddListener(TaskResetHighlight);
		Graphics_Res.onClick.AddListener(TaskResetHighlight);
		
		Main_Settings.onClick.AddListener(TaskVol);
		Sub.onClick.AddListener(TaskSub);
		
		vr = Volume.GetComponent<Image>();
		rr = Resolution.GetComponent<Image>();
		cr = Controls.GetComponent<Image>();
		
		m.onValueChanged.AddListener (delegate { TaskResetHighlight();});
		s.onValueChanged.AddListener (delegate { TaskResetHighlight();});
		d.onValueChanged.AddListener (delegate { TaskResetHighlight();});
		rc.onValueChanged.AddListener (delegate { TaskResetHighlight();});
		
		flag = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(flag == 1) {
			v.SetActive(true);
			r.SetActive(false);
			c.SetActive(false);
			
			vr.color = new Color32(99,90,90,255);
			cr.color = new Color32(255,255,255,255);
			rr.color = new Color32(255,255,255,255);
		}
		else if (flag == 2) {
			v.SetActive(false);
			r.SetActive(true);
			c.SetActive(false);
			
			rr.color = new Color32(99,90,90,255);
			cr.color = new Color32(255,255,255,255);
			vr.color = new Color32(255,255,255,255);
		}
 	    else if (flag == 3) {
			v.SetActive(false);
			r.SetActive(false);
			c.SetActive(true);
			
			cr.color = new Color32(99,90,90,255);
			vr.color = new Color32(255,255,255,255);
			rr.color = new Color32(255,255,255,255);
	    }
		
    }
	
	void TaskVol() {
		flag = 1;
		EventSystem.current.SetSelectedGameObject(null);
	}
	
	void TaskRes() {
		flag = 2;
		EventSystem.current.SetSelectedGameObject(null);
	}
	
	void TaskCon() {
		flag = 3;
		EventSystem.current.SetSelectedGameObject(null);
	}
	
	void TaskResetHighlight() {
		EventSystem.current.SetSelectedGameObject(null);
	}
	
	void TaskSub() {
		isSubtitle = !isSubtitle;
		EventSystem.current.SetSelectedGameObject(null);
		if(isSubtitle)
			onOff.text = "ON";
		else
			onOff.text = "OFF";
	}
}
