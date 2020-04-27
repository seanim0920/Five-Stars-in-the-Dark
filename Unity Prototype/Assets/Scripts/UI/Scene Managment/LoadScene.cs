using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static int prevLevel = 0;
    //Loads a Unity Scene by exact name. All other scenes will be unloaded
    public static void Loader(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
