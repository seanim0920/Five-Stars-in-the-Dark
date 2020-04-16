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
	    var pos = transform.position;
		
		if(transform.position.x >= -100) {
			Instructions.SetActive(false);
			LvlSelect.SetActive(false);
			if(Masterkey.egg) {
				Blackout.SetActive(true);
				LoadScene.Loader("Level 1");
			} else if (Masterkey.lvl) {
                Blackout.SetActive(true);
                LoadScene.Loader(Masterkey.sceneName);
            }
		}
    }
}
