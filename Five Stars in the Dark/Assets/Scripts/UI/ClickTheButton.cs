using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClickTheButton : MonoBehaviour
{
    Button r;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Button>();
		r.onClick.AddListener(Task);
    }
     
	void Task() {
		Masterkey.played = true;	
	}	
}
