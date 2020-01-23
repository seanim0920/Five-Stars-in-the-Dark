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
    private static float timeTillNextCheckpoint;

    public static void SetLastCheckpoint(GameObject checkpoint)
    {
        timeTillNextCheckpoint = 40f;
        lastCheckpoint = checkpoint;
    }
    public static void SetNextTurn(string soundName)
    {
        timeTillNextCheckpoint = 40f;
        nextTurn = soundName;
    }
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
        player = GameObject.Find("Car").transform;
        lastCheckpoint = null;
        newPos = new Vector3(0f, 0f, 0f);
        nextTurn = null;
        errors = 0;
        timeTillNextCheckpoint = 1000f;
        errorText = GameObject.Find("ErrorText").GetComponent<Text>();
        errorText.text = "Error(s): " + errors.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for wrong turns
        /*if (nextTurn != null)
        {
            if ((Input.GetKey("left") && nextTurn.Contains("right")) || (Input.GetKey("right") && nextTurn.Contains("left")))
            {
                RestartFromCheckpoint();
                nextTurn = null;
                // Increment error counter
                CheckErrors.IncrementErrorsAndUpdateDisplay();
            }
        }*/

        // Check if player is taking too long reaching the next checkpoint
        ////////////////////////////////////////////////////////////////////////////////////
        // Code by rutter: https://answers.unity.com/questions/225213/c-countdown-timer.html
        timeTillNextCheckpoint -= Time.deltaTime;
        if (timeTillNextCheckpoint < 0)
        {
            Debug.Log("Recalculating");
            timeTillNextCheckpoint = 40f;
            RestartFromCheckpoint();
            nextTurn = null;
            // Increment error counter
            CheckErrors.IncrementErrorsAndUpdateDisplay();
        }
        //////////////////////////////////////////////////////////////////////////////////////
    }

    void RestartFromCheckpoint()
    {
        // Debug.Log("checkpoint name " + checkpoint.name);
        if (lastCheckpoint == null)
        {
            player.localEulerAngles = new Vector3(0f, 0f, -180f);
            player.position = new Vector3(-13.74f, 1.37f, 0f);
        }
        else
        {
            Debug.Log("lastCheckpoint: " + lastCheckpoint.name);
            Debug.Log("nextTurn: " + nextTurn);
            float angleAdjustment = 0f;
            if (nextTurn == null)
            {
                Debug.Log("nextTurn is null");
            }
            else if (nextTurn.Contains("left"))
            {
                angleAdjustment = 180f;
            }
            newPos = lastCheckpoint.transform.position;
            if (lastCheckpoint.transform.eulerAngles.z == 0)
            {
                newPos += new Vector3(5f, 0f, 0f);
                player.localEulerAngles = new Vector3(0f, 0f, -180f + angleAdjustment); // -180 for right turns
            }
            else if (lastCheckpoint.transform.eulerAngles.z == 90)
            {
                newPos += new Vector3(0f, 5f, 0f);
                player.localEulerAngles = new Vector3(0f, 0f, -90f + angleAdjustment); // -90 for right turns
            }
            else if (lastCheckpoint.transform.eulerAngles.z == 180)
            {
                newPos += new Vector3(-5f, 0f, 0f);
                player.localEulerAngles = new Vector3(0f, 0f, 0f + angleAdjustment); // 0 for right turns
            }
            else
            {
                newPos += new Vector3(0f, -5f, 0f);
                player.localEulerAngles = new Vector3(0f, 0f, 90f + angleAdjustment); // 90 for right turns
            }
            player.position = newPos;
        }
    }
}
