using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    public GameObject fade;
	public GameObject panel;
    public DisplayScore scoreScript;
    // Start is called before the first frame update
    void Start()
    {
        fade.SetActive(true);
        // panel.SetActive(false);
        // scoreScript.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
