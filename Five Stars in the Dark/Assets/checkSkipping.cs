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
        icon.enabled = false;
    }
    void Update()
    {
        icon.enabled = SkipCutscenes.isSkipping;
    }
}
