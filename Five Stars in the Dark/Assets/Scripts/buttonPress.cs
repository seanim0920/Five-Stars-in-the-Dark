using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class buttonPress : MonoBehaviour
{
    public Button instructionsButton;
    public Button playButton;
    public bool state = false;
    public bool pressed = false;
    string[] activeForceAndEffect;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnApplicationQuit()
    {
    }
}
