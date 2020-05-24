using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeSpent : MonoBehaviour
{
    private static Text time;
	public static float remainder = 120f;
	public static float x = 120f;
    // Start is called before the first frame update
    void Start()
    {
        time = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
		remainder = CountdownTimer.levelCompleteTime - CountdownTimer.getCurrentTime();
        remainder /= 2;
        if((remainder%60) > 9)		
			time.text = "TIME: 0" + Mathf.Floor(remainder/60) + ":" + Mathf.Floor(remainder%60);
		else
			time.text = "TIME: 0" + Mathf.Floor(remainder/60) + ":" + Mathf.Floor(remainder%60);
    }
}
