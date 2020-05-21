using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static void Loader(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public static void Loader(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
    }

    public static void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1, LoadSceneMode.Single);
    }
    public static AsyncOperation LoadNextSceneAsync()
    {
        return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
    public static AsyncOperation LoadNextSceneAdditiveAsync()
    {
        return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Additive);
    }
}
