using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour
{
	public Button forward;
	public Button backward;
	public GameObject[] Nodes;
	public GameObject[] Paths;
	public GameObject[] Nodes2;
	public GameObject[] Paths2;
	public GameObject[] Levels;
	public int lvlCount = 0;
    // Start is called before the first frame update
    void Start()
    {
		lvlCount = 0;
        forward.GetComponent<Button>().onClick.AddListener(a);
		backward.GetComponent<Button>().onClick.AddListener(b);
    }

    // Update is called once per frame
    void Update()
    {
		for(int i=0; i<5; i++){
			Nodes[i].SetActive(false);
			Paths[i].SetActive(false);
			Nodes2[i].SetActive(true);
			Paths2[i].SetActive(true);
		}
		for(int i=0; i<lvlCount; i++){
			Nodes[i].SetActive(true);
			Paths[i].SetActive(true);
			Nodes2[i].SetActive(false);
			Paths2[i].SetActive(false);
		}
		for(int i=0; i<6; i++) {
			if(i == lvlCount)
                Levels[i].SetActive(true);
			else
			    Levels[i].SetActive(false);
		}	
    }
	
	void a(){
		lvlCount++;
		if(lvlCount == 6)
			lvlCount = 0;
	}
	
	void b(){
		lvlCount--;
		if(lvlCount == -1)
			lvlCount = 5;
	}
}
