using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class subtitleText : MonoBehaviour
{
    private static Text subText;
    private bool italics = false;
    // Start is called before the first frame update
    void Start()
    {
        ConstructLevelFromMarkers.subtitleMessage = "";
        subText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!string.Equals(subText.text, ConstructLevelFromMarkers.subtitleMessage))
        {
            subText.fontStyle = italics ? FontStyle.Italic : FontStyle.Normal;
            subText.text = matchColorandTrimQuotes(ConstructLevelFromMarkers.subtitleMessage);
        }
    }

    string matchColorandTrimQuotes(string message)
    {
        if (message.Length >= 5)
        {
            //string[] tokens = message.Trim().Split(new char[0], System.StringSplitOptions.RemoveEmptyEntries);
            if (message[0] == '<' && char.ToLower(message[1]) == 'y')
            {
                subText.color = Color.yellow;
            }
            else
            {
                subText.color = Color.white;
            }
            if (char.ToLower(message[2]) == 'i')
            {
                italics = true;
                return message.Substring(6, message.Length - 7);
            }
            else
            {
                italics = false;
                return message.Substring(5, message.Length - 6);
            }
        }
        return "";
    }
}