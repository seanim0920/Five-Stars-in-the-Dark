using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInstructions : MonoBehaviour
{
	public GameObject Instructions;
	public GameObject LevelSelect;
	public bool lvlMode = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		lvlMode = Masterkey.lvl;
	    var pos = transform.position;
		if(transform.position.x <= -100)
			if(lvlMode)
				LevelSelect.SetActive(true);
			else
				Instructions.SetActive(true);
    }
}
