using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthOfCar : MonoBehaviour
{
    public Transform carTransform;

    void Update()
    {
        transform.position = carTransform.position + new Vector3(0, 3, 0);
    }
}