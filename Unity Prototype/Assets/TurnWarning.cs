using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnWarning : MonoBehaviour
{
    //public int direction;
    public bool isRightTurn;
    private AudioSource audioData;
    // Start is called before the first frame update
    void Start()
    {
        // ...
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
    }
    public void WarnTurn(string other)
    {
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // ...
    }

    void OnTriggerExit2D(Collider2D other)
    {
    }
}
