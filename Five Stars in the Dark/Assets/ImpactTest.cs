using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitthenforce());
    }

    IEnumerator waitthenforce()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 50), ForceMode2D.Impulse);
        yield return new WaitForSeconds(1);
        GetComponent<Rigidbody2D>().velocity *= 0;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
