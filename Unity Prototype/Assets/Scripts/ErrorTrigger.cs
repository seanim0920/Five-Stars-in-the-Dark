using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorTrigger : MonoBehaviour
{
    AudioSource errorSource;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        errorSource = GetComponent<AudioSource>();
        sprite = transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("color is " + sprite.color);
        if (other.gameObject.tag == "Player" && sprite.color.r > 0) //checks color of object
        {
            errorSource.Play();
            CheckErrors.IncrementErrorsAndUpdateDisplay();
        }
    }
}
