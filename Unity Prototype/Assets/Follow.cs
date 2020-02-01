using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform focus;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float xpos = 5;

        transform.position = new Vector3(focus.position.x, focus.position.y + 3, -0.3f);
    }
}
