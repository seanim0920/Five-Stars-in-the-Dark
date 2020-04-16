using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //Loads a Unity Scene by exact name. All other scenes will be unloaded
    public static void Loader(string sceneName)
    {
        SceneManager.LoadScene("Level 2", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
