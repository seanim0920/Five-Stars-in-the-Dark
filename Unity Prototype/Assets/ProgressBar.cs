using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    Slider scoreBar;
    // Start is called before the first frame update
    float shakeAmount = 20;
    float shakeOffset = 0;
    float duration = 1f;
    float score = 1f;
    float lerpTime = 0;
    RectTransform rect;
    void Start()
    {
        scoreBar = GetComponentInChildren<Slider>();
        rect = GetComponent<RectTransform>();
        StartCoroutine(IncrementProgress());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(lerpTime);
        scoreBar.value = lerpTime;

        Vector2 displacement = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right * shakeOffset;
        rect.anchoredPosition = Vector2.zero + displacement;
    }

    IEnumerator IncrementProgress()
    {
        while (lerpTime < 0.99f)
        {
            lerpTime = Mathf.Sin(Time.time);
            //float lerpTime = Mathf.PingPong(Time.time, duration) / duration;
            //Debug.Log(lerpTime);
            //smooth interpolation dependso n smothness of time change
            shakeOffset = Mathf.Lerp(shakeAmount, 0, scoreBar.value / score);
            yield return new WaitForSeconds(0);
        }
    }
}
