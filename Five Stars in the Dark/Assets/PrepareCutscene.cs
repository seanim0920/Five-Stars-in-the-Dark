using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareCutscene : MonoBehaviour
{
    public static AsyncOperation asyncOperation;
    // Start is called before the first frame update
    void Start()
    {
        asyncOperation = LoadScene.LoadNextSceneAdditiveAsync();
        //Don't let the Scene activate until you allow it to
        //asyncOperation.allowSceneActivation = false;
    }
}
