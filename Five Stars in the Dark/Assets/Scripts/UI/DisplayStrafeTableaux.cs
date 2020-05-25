using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DisplayStrafeTableaux : MonoBehaviour
{
    private GameObject tableaux;
    private Transform tableauxType;
    private Text proceed;
    private float startTime = 0f;
    public int tableauxNum;
    // Start is called before the first frame update
    void Start()
    {
        tableaux = GameObject.Find("TableauxMockups");
        Debug.Log(tableaux);
        tableauxType = tableaux.transform.GetChild(tableauxNum);
        Debug.Log(tableauxType);
        tableauxType.gameObject.SetActive(true);
        proceed = tableaux.GetComponentInChildren<Text>();
        Time.timeScale = 0f;
        // AudioListener.pause = true;
        startTime = Time.unscaledTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if 3 seconds have elapsed && Accelerate to continue text is not enabled
        if(Time.unscaledTime - startTime > 3f && !proceed.enabled)
        {
            // if so, enable "Accelerate to continue" text
            proceed.enabled = true;
        }
        // Check if "Accelerate to continue" is being displayed, and player presses the button
        if(proceed.enabled && (Input.GetKeyDown("up") || (Gamepad.current != null && Gamepad.current.rightTrigger.wasPressedThisFrame)))
        {
            // if so, unpause the game and destroy this object
            tableauxType.gameObject.SetActive(false);
            proceed.enabled = false;
            Time.timeScale = 1f;
            // AudioListener.pause = false;
            Destroy(gameObject);
        }
    }
}
