using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipeSeppuku : MonoBehaviour
{
	public GameObject Settings;
	public GameObject Wipe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
		
		if(transform.position.x >= -1500) {
			Settings.SetActive(false);
            if(transform.position.x >= 1000) {
				Wipe.SetActive(false);
			}
		}
    }
}
