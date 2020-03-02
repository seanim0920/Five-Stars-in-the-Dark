using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
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
		remainder = CountdownTimer.currentTime;
        if(((120 - remainder)%60) > 9)		
			time.text = "TIME: 0" + Mathf.Floor(((120 - remainder)/60)) + ":" + ((120 - remainder)%60);
		else
			time.text = "TIME: 0" + Mathf.Floor(((120 - remainder)/60)) + ":0" + ((120 - remainder)%60);
    }
}
