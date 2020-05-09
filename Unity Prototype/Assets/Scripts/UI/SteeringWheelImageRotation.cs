using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteeringWheelImageRotation : MonoBehaviour
{
    public AudioSource disabledWheelSound;
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
        if (PlayerControls.enabled)
        {
            changeWheelImageAngle();
        }
        wheelImage.rectTransform.rotation = Quaternion.Euler(0, 0, wheelAngle);
    }
    public void changeWheelImageAngle()
    {
        wheelAngle = PlayerControls.getStrafeAmount() * -443;
    }
    public IEnumerator turnFail(bool right)
    {
        disabledWheelSound.Play();
        float inc = 1;
        if (right)
        {
            inc = -1;
        }
        for (int i = 0; i < 90; i++)
        {
            print("trying to turn right");
            wheelAngle += inc;
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 90; i++)
        {
            wheelAngle -= inc;
            yield return new WaitForFixedUpdate();
        }
    }
}