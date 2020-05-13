using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class printLevelDebugText : MonoBehaviour
{
    private static Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        debugText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = ConstructLevelFromMarkers.debugMessage;
    }
}