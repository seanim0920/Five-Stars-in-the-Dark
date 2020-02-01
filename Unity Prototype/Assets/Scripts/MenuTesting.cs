using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTesting : MonoBehaviour
    
{
    //The only thing this script does is print 'Ping!' to the console when the public method is called.
    public void PingMe()
    {
        Debug.Log("Ping!");
    }

    public void PingMeTwo()
    {
        Debug.Log("PING ME BOY");
    }

    public void QuitGame()
    {
        Debug.Log("This would quite the game, but Application.Quit doesn't work in editor");
        Application.Quit();
    }
}
