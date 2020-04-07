using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    public AudioSource bass;
    public AudioSource drums;
    public GameObject fade;
	public GameObject panel;
    public AudioSource endScene;
    public DisplayScore script;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayScene()
    {
        endScene.Play();
        yield return new WaitForSeconds(endScene.clip.length);
        fade.SetActive(true);
		panel.SetActive(false);
		yield return new WaitForSeconds(1);
        script.enabled = true;
        bass.volume = 1;
        drums.volume = 1;
    }
}
