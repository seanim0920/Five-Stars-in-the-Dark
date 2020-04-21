using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
    private float startTime;
    Slider scoreBar;
    // Start is called before the first frame update
    float shakeAmount = 10;
    float shakeOffset = 0;
    float duration = 1f;
    public static float score = Mathf.Exp(-CheckErrors.errors/3);
    float lerpTime = 0;
    RectTransform rect;

    //Text fields to display to on the end screen
    Text timeText;
    Text progressText;
    Text errorText;

    void Start()
    {
        startTime = Time.time;
        scoreBar = GetComponentInChildren<Slider>();
        rect = GetComponent<RectTransform>();

        //Grabbing text fields. They're in the same place in both End and Fail
        //   so I'm hardcoding it.
        //3 denotes the ScoreUI object
        progressText = GetGrandchild(this.gameObject, 3, 0).GetComponent<Text>();   //0 is Stat0 [progress]
        errorText = GetGrandchild(this.gameObject, 3, 2).GetComponent<Text>();      //2 is ERRORS
        timeText = GetGrandchild(this.gameObject, 3, 4).GetComponent<Text>();       //4 is TIME
        //Set text field texts to score
        Debug.Log("Stored Progress: " + ScoreStorage.Instance.getScoreProgress());
        if(ScoreStorage.Instance.getScoreProgress() >= 100)
        {
            progressText.text = "THANKS FOR PLAYING";
        }
        else
        {
            progressText.text = "PROGRESS: " + ScoreStorage.Instance.getScoreProgress() + "%";
        }
        errorText.text = "ERRORS: " + ScoreStorage.Instance.getScoreErrors();
        timeText.text = "TIME: " + ScoreStorage.Instance.getScoreTimeFormatted();

        int prog = 0;

        StartCoroutine(IncrementProgress());

        float scoreBonus = 0;
        if (score > .6)
        {
            score = .6f;
            scoreBonus = Mathf.Exp((-CountdownTimer.currentTime) / 6);
            if(scoreBonus > .4)
            {
                scoreBonus = .4f;
            }
        }
        score += scoreBonus;

        //Hi. Thomas Here. I don't know what scripts (if any)
        //   update the progress and time fields of the endscreen,
        //   so I'm just putting it here.
        /*prog = (current point in dialogue *100) / end point of dialogue;
        if(prog == 100)
        {
            //We must be on EndScreen because the player beat the level
            progressText = "THANKS FOR PLAYING";
        }
        else
        {
            //We must be on Failscreen because the player didn't finish
            progressText = "PROGRESS: " + prog;
        }
        errorText = number of errors;
        timeText = the rime correctly formatted;*/
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(lerpTime);
        scoreBar.value = lerpTime * score;

        Vector2 displacement = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right * shakeOffset;
        rect.anchoredPosition = Vector2.zero + displacement;
    }

    IEnumerator IncrementProgress()
    {
        while (lerpTime < 0.99f)
        {
            lerpTime = Mathf.Sin(Time.time - startTime);
            //float lerpTime = Mathf.PingPong(Time.time, duration) / duration;
            //Debug.Log(lerpTime);
            //smooth interpolation dependso n smothness of time change
            shakeOffset = Mathf.Lerp(shakeAmount, 0, lerpTime);
            yield return new WaitForSeconds(0);
        }
    }

    //returns go's gradchild at index grandchild by pulling it from go's child at index child
    GameObject GetGrandchild(GameObject go, int child, int grandchild)
    {
        return go.transform.GetChild(child).GetChild(grandchild).gameObject;
    }
}
