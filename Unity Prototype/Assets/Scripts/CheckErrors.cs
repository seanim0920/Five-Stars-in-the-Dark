using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckErrors : MonoBehaviour
{
    public AudioClip errorInit;
    public static AudioClip errorSound;
    public static Transform player;
    public static Text errorText;
    private static GameObject lastCheckpoint;
    private static string nextTurn;
    private static int errors;
    private Vector3 newPos;

    public static void IncrementErrorsAndUpdateDisplay()
    {
        errors++;
        errorText.text = "Error(s): " + errors.ToString();
        //AudioSource.PlayClipAtPoint(errorSound, player.position);
    }
    // Start is called before the first frame update
    void Start()
    {
        errorSound = errorInit;
        errors = 0;
        errorText = GameObject.Find("ErrorText").GetComponent<Text>();
        errorText.text = "Error(s): " + errors.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
