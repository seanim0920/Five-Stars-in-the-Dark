using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ReplaceButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ContextMenu("Replace")]
    void replace()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name.Contains("Button_"))
            {
                GameObject button = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Prefabs/UI/MenuButton"));
                button.name = go.name;
                button.SetActive(go.activeSelf);
                button.transform.parent = go.transform.parent;
                button.transform.localPosition = go.transform.localPosition;
                button.transform.localRotation = go.transform.localRotation;
                button.transform.localScale = go.transform.localScale;
                if (go.GetComponentInChildren<Text>() != null)
                {
                    button.GetComponentInChildren<Text>().text = go.GetComponentInChildren<Text>().text;
                    button.transform.GetChild(0).localPosition = go.transform.GetChild(0).localPosition;
                    button.transform.GetChild(0).localRotation = go.transform.GetChild(0).localRotation;
                    button.transform.GetChild(0).localScale = go.transform.GetChild(0).localScale;
                }
                //ignore transform and scale
            }
        }
    }

    [ContextMenu("ReplaceNav")]
    void replaceNav()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name.Contains("Button_"))
            {
                Navigation navClone = go.GetComponent<Button>().navigation;
                navClone.selectOnUp = go.GetComponent<Button>().FindSelectableOnUp();
                navClone.selectOnLeft = go.GetComponent<Button>().FindSelectableOnLeft();
                navClone.selectOnRight = go.GetComponent<Button>().FindSelectableOnRight();
                navClone.selectOnDown = go.GetComponent<Button>().FindSelectableOnDown();
                go.GetComponent<Button>().navigation = navClone;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
