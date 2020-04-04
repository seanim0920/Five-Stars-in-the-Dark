using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTitle : MonoBehaviour
{

    public GameObject Instructions;
	public GameObject LvlSelect;
	public GameObject Blackout;
	public bool f = false;
    // Start is called before the first frame update
    void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
		f = Masterkey.egg;
	    var pos = transform.position;
		
		if(transform.position.x >= -100) {
			Instructions.SetActive(false);
			LvlSelect.SetActive(false);
			if(f) {
				Blackout.SetActive(true);
				LoadScene.Loader("Level 1");
			}
		}
    }
}
