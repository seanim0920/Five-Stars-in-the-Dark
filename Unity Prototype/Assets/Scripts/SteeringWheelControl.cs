using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SteeringWheelControl : MonoBehaviour
{
    LogitechGSDK.LogiControllerPropertiesData properties;
    private string actualState;
    private string activeForces;
    private string propertiesEdit;
    private string buttonStatus;
    private string forcesLabel;
    private PlayerControls controlFunctions;
    private KeyboardControl keyboardScript;
    string[] activeForceAndEffect;
    // Start is called before the first frame update
    void Start()
    {
        LogitechGSDK.LogiSteeringInitialize(false);
        controlFunctions = GetComponent<PlayerControls>();
        keyboardScript = GetComponent<KeyboardControl>();
    }

    // Update is called once per frame
    void Update()
    {
        keyboardScript.enabled = true;
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            keyboardScript.enabled = false;
            LogitechGSDK.LogiPlaySpringForce(0, 0, 70, 25);
            //coefficientPercentage : specify the slope of the effect strength increase relative to the amount of deflection from the center of the condition. Higher values mean that the saturation level is reached sooner. Valid ranges are -100 to 100. Any value outside the valid range is silently clamped. -100 simulates a very slippery effect, +100 makes the wheel/joystick very hard to move, simulating the car at a stop or in mud.
            //LogitechGSDK.LogiPlayDamperForce(0, 100);
            //CONTROLLER STATE
            actualState = "Steering wheel current state : \n\n";
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            actualState += "x-axis position :" + rec.lX + "\n";
            actualState += "z-axis rotation :" + rec.lRz + "\n";
            actualState += "extra axes positions 1 :" + rec.rglSlider[0] + "\n";

            if (rec.lY < 0)
            {
                controlFunctions.speedUp(1f);
            }
            else if (rec.lRz < 0)
            {
                controlFunctions.slowDown(0.02f);
            }
            else
            {
                controlFunctions.returnToNeutralSpeed();
            }

            controlFunctions.strafe(rec.lX / 32768f);
        }
    }

    public void PlayDirtRoadForce(int useableRange)
    {
        LogitechGSDK.LogiPlayDirtRoadEffect(0, useableRange);
    }
    public void StopDirtRoadForce()
    {
        LogitechGSDK.LogiStopDirtRoadEffect(0);
    }

    public void PlaySoftstopForce(int useableRange)
    {
        LogitechGSDK.LogiPlaySoftstopForce(0, useableRange);
    }

    public void StopSoftstopForce()
    {
        LogitechGSDK.LogiStopSoftstopForce(0);
    }

    public void PlayFrontCollisionForce()
    {
        LogitechGSDK.LogiPlayFrontalCollisionForce(0, 100);
    }

    void OnApplicationQuit()
    {
        Debug.Log("SteeringShutdown:" + LogitechGSDK.LogiSteeringShutdown());
    }
}
