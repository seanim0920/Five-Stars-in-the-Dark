using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SteeringWheelInput : MonoBehaviour
{
    LogitechGSDK.LogiControllerPropertiesData properties;
    private string actualState;
    private string activeForces;
    private string propertiesEdit;
    private string buttonStatus;
    private string forcesLabel;
    private PlayerControls controlFunctions;
    string[] activeForceAndEffect;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            LogitechGSDK.LogiSteeringInitialize(false);
        }
        catch
        {
            print("steering wheel aint working");
        }
        controlFunctions = GetComponent<PlayerControls>();
    }

    public static bool checkConnected()
    {
        try
        {
            return (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0));
        }
        catch
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (checkConnected())
        {
            LogitechGSDK.LogiPlaySpringForce(0, 0, 20, 85);
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

    public void PlaySideCollisionForce(int magnitude)
    {
        if (checkConnected())
            LogitechGSDK.LogiPlaySideCollisionForce(0, magnitude);
    }
    public void StopSideCollisionForce()
    {
        //LogitechGSDK.LogiStopSideCollisionForce(0);
    }
    public void PlayDirtRoadForce(int useableRange)
    {
        if (checkConnected())
            LogitechGSDK.LogiPlayDirtRoadEffect(0, useableRange);
    }
    public void StopDirtRoadForce()
    {
        if (checkConnected())
            LogitechGSDK.LogiStopDirtRoadEffect(0);
    }

    public void PlaySoftstopForce(int useableRange)
    {
        if (checkConnected())
            LogitechGSDK.LogiPlaySoftstopForce(0, useableRange);
    }

    public void StopSoftstopForce()
    {
        if (checkConnected())
            LogitechGSDK.LogiStopSoftstopForce(0);
    }

    public void PlayFrontCollisionForce()
    {
        if (checkConnected())
            LogitechGSDK.LogiPlayFrontalCollisionForce(0, 100);
    }

    void OnApplicationQuit()
    {
        Debug.Log("SteeringShutdown:" + LogitechGSDK.LogiSteeringShutdown());
    }
}
