using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedShake : MonoBehaviour
{
    private PlayerControls controls;
    float shakeOffset = 5;
    float lerpTime = 0;
    public RectTransform rect;
    Vector2 originalPosition;
    void Start()
    {
        controls = GameObject.Find("Player").GetComponent<PlayerControls>();
        originalPosition = rect.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (controls.enabled)
        {
            Vector2 displacement = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right * shakeOffset * (controls.movementSpeed / controls.maxSpeed);
            rect.anchoredPosition = originalPosition + displacement;
        }
    }
}