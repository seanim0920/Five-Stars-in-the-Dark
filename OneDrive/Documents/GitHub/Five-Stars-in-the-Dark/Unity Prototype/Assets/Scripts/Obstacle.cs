using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>  
///  Attach this script to 
/// </summary>  
public class Obstacle 
{
    ///---PUBLIC---///
    public string Name { get; set; }
    public int ID { get; set;}

    ///---PRIVATE---///
    private Transform _transform;
    private Vector3 _postion;
    private int triggerRadius;
    private int soundLength;
    private bool isActive = false;
    void Start()
    {
        _postion = _transform.position;
    }

    void Update()
    {

        if (isActive == false)
        {
            // set go to not active
        }
        
    }
}



