using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorCountDisplay : MonoBehaviour
{
	private Text errorText;
    // Start is called before the first frame update
    void Start()
    {
        errorText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        errorText.text = "Errors: " + TrackErrors.getErrors().ToString();
    }
}
