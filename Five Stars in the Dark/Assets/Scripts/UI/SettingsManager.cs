using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

// This class keeps track of the settings throughout each scene
public static class SettingsManager
{
    // Music, SFX, Dialogue, Radio + Audio Cues
    public static float[] volumes = {1f, 1f, 1f, 1f};
    // Steering Wheel, Keyboard, Gamepad
    public static bool[] toggles = {LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0), false, Gamepad.current != null};

    public static void setToggles()
    {
        // Give priority to steering wheel controls
        if(toggles[0])
        {
            toggles[1] = false;
            toggles[2] = false;
        }
        // Then Gamepad controls
        else if(toggles[2])
        {
            toggles[1] = false;
        }
        // Otherwise only keyboard controls are
        // toggled and we don't need to do anything
    }
}
