using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>  
///  Check if player is on road surface
/// </summary>  
public class CollisionDebug : MonoBehaviour
{
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine (ray.origin, hitInfo.point, Color.red);
        } else 
        {
        }
    }
}
