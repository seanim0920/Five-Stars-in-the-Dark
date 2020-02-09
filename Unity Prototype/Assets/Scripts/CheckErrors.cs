using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
<<<<<<< HEAD
using UnityEngine;
=======
>>>>>>> origin/Ben
using UnityEngine.SceneManagement;

public class CheckErrors : MonoBehaviour
{
    //public AudioClip errorInit;
    //public static AudioClip errorSound;
    public static Transform player;
    private static Text errorText;
    private static int maxErrors = 10;

    public static int errors { get; set; }

    public static void IncrementErrorsAndUpdateDisplay()
    {
        errors++;
        updateDisplay();
        if (errors > maxErrors)
        {
            SceneManager.LoadScene("EndScreen", LoadSceneMode.Single);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        }
        //AudioSource.PlayClipAtPoint(errorSound, player.position);
    }
    // Start is called before the first frame update
    void Start()
    {
        //errorSound = errorInit;
        errors = 0;
        errorText = GetComponent<Text>();
        updateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private static void updateDisplay()
    {
        errorText.text = "Number of Errors: " + errors.ToString();
    }
}
