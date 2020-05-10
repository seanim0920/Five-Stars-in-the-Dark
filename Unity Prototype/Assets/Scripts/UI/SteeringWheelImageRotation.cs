using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteeringWheelImageRotation : MonoBehaviour
{
    public Image wheelImage;
    private float wheelAngle;
    // Start is called before the first frame update
    void Start()
    {
        changeWheelImageAngle();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        changeWheelImageAngle();
        wheelImage.rectTransform.rotation = Quaternion.Euler(0, 0, wheelAngle);
    }
    public void changeWheelImageAngle()
    {
        wheelAngle = PlayerControls.getStrafeAmount() * -443;
    }
}