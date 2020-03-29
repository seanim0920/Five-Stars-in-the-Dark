using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ErrorCountDisplay : MonoBehaviour
{
	private static Text err;
	public static int errors;
    // Start is called before the first frame update
    void Start()
    {
        err = GetComponent<Text>();
		//errors = error_count.GetComponent<CheckErrors>().errors;
		errors = CheckErrors.errors;
    }

    // Update is called once per frame
    void Update()
    {
        err.text = "ERRORS: " + errors;
    }
}
