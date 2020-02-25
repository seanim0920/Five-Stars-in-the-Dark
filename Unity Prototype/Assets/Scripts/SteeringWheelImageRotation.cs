using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteeringWheelImageRotation : MonoBehaviour
{
    public Image wheelImage;
    public Control1D wheelScript;
    private float wheelAngle;
    // Start is called before the first frame update
    void Start()
    {
        changeWheelImageAngle();
    }

    // Update is called once per frame
    void Update()
    {
        changeWheelImageAngle();
    }
    public void changeWheelImageAngle()
    {
        wheelImage.rectTransform.rotation = Quaternion.Euler(0, 0, wheelScript.GetWheelAngle());
    }
}
