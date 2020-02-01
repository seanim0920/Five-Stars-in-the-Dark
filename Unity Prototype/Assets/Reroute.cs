using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reroute : MonoBehaviour
{
    public GameObject TurnWarning;
    public int streets = 0;
    AudioSource audioData;
    SpriteRenderer sprite;
    GameObject NPC;
    private AudioClip finish;
    private AudioClip wrong;
    // Start is called before the first frame update
    void Start()
    {
        NPC = Resources.Load<GameObject>("NPC");
        finish = Resources.Load<AudioClip>("Audio/gpsend");
        wrong = Resources.Load<AudioClip>("Audio/gpsrecalculating");
        audioData = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.transform.position -= new Vector3(0, 90, 0);
        if (col.gameObject.tag == "Player")
        {
            if ((col.gameObject.transform.position.x < transform.position.x && TurnWarning.tag == "Left") ||
                (col.gameObject.transform.position.x >= transform.position.x && TurnWarning.tag == "Right")) {
                print("right trun");
                if (streets <= 0) {
                    audioData.clip = finish;
                    audioData.Play();
                }
                streets -= 1;
                print(streets);
            } else
            {
                print("wrong trun");
                audioData.clip = wrong;
                audioData.Play();
            }
            TurnWarning.tag = "Left";
            int direction = Random.Range(0, 2);
            if (direction == 1)
            {
                TurnWarning.tag = "Right";
            }
            StartCoroutine(SpawnCars(col.gameObject.transform));
        }
    }
    IEnumerator SpawnCars(Transform playerTransform)
    {
        for (int i = 0; i < 1; i++)
        {
            float xpos = 4;
            int lane = Random.Range(0, 2);
            if (lane == 1)
            {
                xpos = 6;
            }
            GameObject car = Instantiate(NPC, new Vector3(xpos, playerTransform.position.y + 6, 0), Quaternion.identity);
            car.GetComponent<Movement1D>().setSpeed(Random.Range(0.01f, 0.1f));
            yield return new WaitForSeconds(Random.Range(0.1f, 3.0f));
        }
    }
}
