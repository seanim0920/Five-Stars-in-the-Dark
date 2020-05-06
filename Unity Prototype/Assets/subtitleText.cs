using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class subtitleText : MonoBehaviour
{
    private static Text subText;
    // Start is called before the first frame update
    void Start()
    {
        subText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        subText.text = ConstructLevelFromMarkers.subtitleMessage;
    }
}