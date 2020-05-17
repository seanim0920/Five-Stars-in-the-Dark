using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndscreenWipe : MonoBehaviour
{
   	public GameObject Blackout;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
	    var pos = transform.position;
		if(transform.position.x <= -1500) {
			gameObject.SetActive(false);
			Blackout.SetActive(true);
		}
    }
}
