using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedShake : MonoBehaviour
{
    public PlayerControls controls;
    float shakeOffset = 5;
    float lerpTime = 0;
    public RectTransform rect;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (controls.enabled)
        {
            Vector2 displacement = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right * shakeOffset * (controls.movementSpeed / controls.maxSpeed);
            rect.anchoredPosition = Vector2.zero + displacement;
        }
    }
}