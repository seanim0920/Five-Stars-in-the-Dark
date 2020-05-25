using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInstructions : MonoBehaviour
{
	public GameObject Instructions;
	public GameObject LevelSelect;
	public GameObject Settings;
	
    public int flag = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		flag = Masterkey.flag;
	    var pos = transform.position;
		if(transform.position.x <= -100)
			if(flag == 2)
				LevelSelect.SetActive(true);
			else if (flag == 1)
				Instructions.SetActive(true);
			else
				Settings.SetActive(true);
    }
}
