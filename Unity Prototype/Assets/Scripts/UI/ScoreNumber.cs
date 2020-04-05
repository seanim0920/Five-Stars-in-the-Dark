using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreNumber : MonoBehaviour
{
	private static Text scoreText;
	public static float score;
    // Start is called before the first frame update
    void Start()
    {
		 scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
		score = DisplayScore.score;
		if((score*100)%20 != 0)
			scoreText.text = "FINAL SCORE: " + ((score*100)/20).ToString("F2") + "/5";
		else
			scoreText.text = "FINAL SCORE: " + ((score*100)/20) + "/5";
    }
}
