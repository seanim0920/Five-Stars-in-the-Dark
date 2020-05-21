using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkSkipping : MonoBehaviour
{
    public Text icon;
    // Update is called once per frame
    private void Start()
    {
        //these values probs shouldnt be changed here but it works so whatevs we'll keep it for now
        SkipCutscenes.isSkipping = false;
        SkipMovies.isSkipping = false;
        icon.enabled = false;
    }
    void Update()
    {
        icon.enabled = ((SkipCutscenes.isSkipping || SkipMovies.isSkipping) && !PauseMenu.isPaused);
    }
}
