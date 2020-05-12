using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckLosing : MonoBehaviour
{
    private PlayerControls controls;
    private bool lost = false;
    AudioSource soundSource;
    public AudioSource secondSoundSource;

    private void Start()
    {
        controls = GameObject.Find("Player").GetComponent<PlayerControls>();
        soundSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!lost && (CountdownTimer.getCurrentTime() <= 0 || TrackErrors.getErrors() >= 10))
        {
            lost = true;
            print("lose!");
            ScoreStorage.Instance.setScoreAll();
            MasterkeyFailScreen.currentLevel = SceneManager.GetActiveScene().name;
            StartCoroutine(failScreenSwitch());
        }
    }

    IEnumerator failScreenSwitch()
    {
        controls.gameObject.tag = "Totalled";
        StartCoroutine(controls.shutOff());
        //if we stop it, carcollisions can restart the clip again after the collision is finished
        ConstructLevelFromMarkers.levelDialogue.volume = 0;
        yield return new WaitForSeconds(4);
        //pick a random soundsource from the set of failure dialogues for this level.
        AudioClip[] failClips = Resources.LoadAll<AudioClip>(SceneManager.GetActiveScene().name + "/Failure");
        if (failClips.Length > 0)
        {
            soundSource.clip = failClips[Random.Range(0, failClips.Length)];
            soundSource.Play();
            yield return new WaitWhile(() => soundSource.isPlaying);
        }

        yield return new WaitForSeconds(0.5f);
        secondSoundSource.Play();

        LoadScene.Loader("FailScreen");
    }
}