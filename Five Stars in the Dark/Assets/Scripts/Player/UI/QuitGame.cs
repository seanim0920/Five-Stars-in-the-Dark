using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
   Button r;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Button>();
		r.onClick.AddListener(TaskEnd);
    }
     
	void TaskEnd() {
		Application.Quit();	
	}	
}
