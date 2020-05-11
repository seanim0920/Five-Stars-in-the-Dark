using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrackErrors : MonoBehaviour
{
    public AudioClip errorInit;
    public static AudioClip errorSound;
    public static Transform player;
    private static GameObject lastCheckpoint;
    private static string nextTurn;
    private Vector3 newPos;

    //set to public so it can be accessed from the end screen
    public static int errors { get; set; }

    public static void IncrementErrorsAndUpdateDisplay()
    {
        errors++;
        ScoreStorage.Instance.setScoreErrors(errors);
        /*if(errors >= 10)
        {
            //IEnumerator failScreen = failScreenSwitch;
            ScoreStorage.Instance.setScoreAll();
            MasterkeyFailScreen.sceneName = SceneManager.GetActiveScene().name;
            //I moved the error scene loading to this coroutine for various reasons
            StartCoroutine(failScreenSwitch());
        }*/
        //AudioSource.PlayClipAtPoint(errorSound, player.position);
    }
    // Start is called before the first frame update
    void Start()
    {
        errorSound = errorInit;
        errors = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (errors >= 10)
        {
            ScoreStorage.Instance.setScoreAll();
            errors = -1;    //This prevents this method from starting every frame, while not changing the actual error count
            MasterkeyFailScreen.currentLevel = SceneManager.GetActiveScene().name;
            //I moved the error scene loading to this class for various reasons
            //hey thomas this is really fucking cursed. yeah well this all i've got so
            failSoundPlay fsp = this.gameObject.AddComponent<failSoundPlay>() as failSoundPlay;
            fsp.playFail();
        }
    }

    //because errors is static, it needs a method to access
    public static float getErrors()
    {
        return errors;
    }
}

//This entire implementation is a sin in several religions (I think), but it lets me
//  run a coroutine through a static function so we take those
class failSoundPlay : MonoBehaviour
{

    AudioSource soundSource;

    public void playFail()
    {
        soundSource = this.gameObject.GetComponent<AudioSource>();
        StartCoroutine(failScreenSwitch());
    }

    IEnumerator failScreenSwitch()
    {
        //The full implementation for this is way out of the scope of this task, so I'm just gonna set 1 faildialogue per scene and be done with it
        soundSource.Play();
        yield return new WaitWhile(() => soundSource.isPlaying);
        LoadScene.Loader("FailScreen");
    }
}
