using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    public AudioSource bass;
    public AudioSource drums;
    public GameObject fade;
	public GameObject panel;
    public DisplayScore scoreScript;
    // Start is called before the first frame update
    void Start()
    {
        bass.time = 16;
        drums.time = 16;
        bass.Play();
        drums.Play();
        fade.SetActive(true);
        // panel.SetActive(false);
        // scoreScript.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
