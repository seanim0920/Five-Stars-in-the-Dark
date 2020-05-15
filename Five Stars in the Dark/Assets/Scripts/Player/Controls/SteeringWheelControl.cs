using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SteeringWheelControl : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaySideCollisionForce(int magnitude)
    {
    }
    public void StopSideCollisionForce()
    {
        //LogitechGSDK.LogiStopSideCollisionForce(0);
    }
    public void PlayDirtRoadForce(int useableRange)
    {
    }
    public void StopDirtRoadForce()
    {
    }

    public void PlaySoftstopForce(int useableRange)
    {
    }

    public void StopSoftstopForce()
    {
    }

    public void PlayFrontCollisionForce()
    {
    }

    void OnApplicationQuit()
    {
    }
}
