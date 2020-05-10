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
        if (ConstructLevelFromMarkers.subtitleMessage.Length > 0 && !string.Equals(subText.text, ConstructLevelFromMarkers.subtitleMessage))
        {
            subText.text = matchColorandTrimQuotes(ConstructLevelFromMarkers.subtitleMessage);
        }
    }

    string matchColorandTrimQuotes(string message)
    {
        if (message[0] == '<' && char.ToLower(message[1]) == 'Y')
        {
            subText.color = Color.yellow;
        }
        return message.Substring(4).Trim('"');
    }
}