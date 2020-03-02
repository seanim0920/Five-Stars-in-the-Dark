using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    public AudioSource intro;
    private PlayerControls controls;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startLevel());
        controls = GetComponent<PlayerControls>();
    }

    IEnumerator startLevel()
    {
        intro.Play();
        yield return new WaitForSeconds(intro.clip.length);
        controls.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
