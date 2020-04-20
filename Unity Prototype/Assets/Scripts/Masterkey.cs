using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Masterkey : MonoBehaviour
{
	public Button start;
	public Button level;
	public Button levelBack;
	public static bool egg = false;
	public static bool lvl = false;
    public static string sceneName = "Level 1";
    public Button level1;
    public Button level2;
    public Button level3;
    public Button level4;

    // Start is called before the first frame update
    void Start()
    {
		Button e = start.GetComponent<Button>();
		Button l = level.GetComponent<Button>();
		Button lb = levelBack.GetComponent<Button>();
        e.onClick.AddListener(() => egg = true);
		l.onClick.AddListener(() => lvl = true);
		lb.onClick.AddListener(() => lvl = false);

        Button l1 = level1.GetComponent<Button>();
        Button l2 = level2.GetComponent<Button>();
        Button l3 = level3.GetComponent<Button>();
        Button l4 = level4.GetComponent<Button>();
        l1.onClick.AddListener(() => sceneName = "Level 1");
        l2.onClick.AddListener(() => sceneName = "Level 2");
        l3.onClick.AddListener(() => sceneName = "Level 3");
        l4.onClick.AddListener(() => sceneName = "Level 4");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
