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
    private Control1D controlFunctions;
    string[] activeForceAndEffect;
    // Start is called before the first frame update
    void Start()
    {
        controlFunctions = GetComponent<Control1D>();
    }

    // Update is called once per frame
    void Update()
    {
        //All the test functions are called on the first device plugged in(index = 0)
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            //coefficientPercentage : specify the slope of the effect strength increase relative to the amount of deflection from the center of the condition. Higher values mean that the saturation level is reached sooner. Valid ranges are -100 to 100. Any value outside the valid range is silently clamped. -100 simulates a very slippery effect, +100 makes the wheel/joystick very hard to move, simulating the car at a stop or in mud.
            LogitechGSDK.LogiPlayDamperForce(0, 100);
            //CONTROLLER STATE
            actualState = "Steering wheel current state : \n\n";
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            actualState += "x-axis position :" + rec.lX + "\n";
            actualState += "z-axis rotation :" + rec.lRz + "\n";
            actualState += "extra axes positions 1 :" + rec.rglSlider[0] + "\n";

            if (rec.lY < 0)
            {
                controlFunctions.speedUp();
            } else if (rec.lRz < 0)
            {
                controlFunctions.slowDown();
            }
            else
            {
                controlFunctions.returnToNeutralSpeed();
            }

            controlFunctions.strafe(Mathf.Round((int)(500 * (rec.lX / 7000f))) / 500);
        }
        else if (!LogitechGSDK.LogiIsConnected(0))
        {
            actualState = "PLEASE PLUG IN A STEERING WHEEL OR A FORCE FEEDBACK CONTROLLER";
        }
        else
        {
            actualState = "THIS WINDOW NEEDS TO BE IN FOREGROUND IN ORDER FOR THE SDK TO WORK PROPERLY";
        }
    }
    public void PlaySoftstopForce(int useableRange)
    {
        LogitechGSDK.LogiPlaySoftstopForce(0, useableRange);
    }

    public void StopSoftstopForce()
    {
        LogitechGSDK.LogiStopSoftstopForce(0);
    }

    void OnApplicationQuit()
    {
        Debug.Log("SteeringShutdown:" + LogitechGSDK.LogiSteeringShutdown());
    }
}
