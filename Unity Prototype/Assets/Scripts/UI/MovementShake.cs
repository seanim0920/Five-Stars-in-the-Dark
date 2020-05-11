using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementShake : MonoBehaviour
{
    private PlayerControls controls;
    public static float shakeOffset = 5;
    float lerpTime = 0;
    public RectTransform rect;
    Vector2 originalPosition;
    Vector2 modifiedPosition;
    void Start()
    {
        controls = GameObject.Find("Player").GetComponent<PlayerControls>();
        originalPosition = rect.anchoredPosition;
        modifiedPosition = originalPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (controls.enabled)
        {
            Vector2 displacement = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right * shakeOffset * (controls.movementSpeed / controls.maxSpeed);
            rect.anchoredPosition = modifiedPosition + displacement;

            modifiedPosition.x = controls.getStrafeAmount() * 150;
            modifiedPosition.y = originalPosition.y + 200*(controls.movementSpeed / controls.maxSpeed);
            rect.localScale = new Vector3(1.0f + (controls.movementSpeed / controls.maxSpeed), 1.0f + (controls.movementSpeed / controls.maxSpeed), 1);
            
        }
    }
}