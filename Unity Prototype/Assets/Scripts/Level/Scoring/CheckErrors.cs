using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckErrors : MonoBehaviour
{
    public AudioClip errorInit;
    public static AudioClip errorSound;
    public static Transform player;
    public static Text errorText;
    private static GameObject lastCheckpoint;
    private static string nextTurn;
    private Vector3 newPos;

    //set to public so it can be accessed from the end screen
    public static int errors { get; set; }

    public static void IncrementErrorsAndUpdateDisplay()
    {
        errors++;
        ScoreStorage.Instance.setScoreErrors(errors);
        if(errors >= 10)
        {
            ScoreStorage.Instance.setScoreAll();
            MasterkeyFailScreen.currentLevel = SceneManager.GetActiveScene().name;
            LoadScene.Loader("FailScreen");
        }
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

    //because errors is static, it needs a method to access
    public int getErrors()
    {
        return errors;
    }
}
