using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndscreenWipe : MonoBehaviour
{
   	public GameObject Blackout;
	public int mode = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		mode = Masterkey2.flag;
	    var pos = transform.position;
		if(transform.position.x <= -1500) {
			if(mode == 1)
				LoadScene.Loader("Level 1");
			else if (mode == 2)
				LoadScene.Loader("Level 1");
			else if (mode == 3)
				LoadScene.Loader("Menu");
			gameObject.SetActive(false);
			Blackout.SetActive(true);
		}
    }
}
