using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>  
///  Controller for car speed, turn and stopping.
/// </summary>  
public class Movement : MonoBehaviour {
    ///---PUBLIC---///
    //public NavMeshAgent agent;
    public float movementSpeed = -100f;
    public float rotationSpeed = 90.0f;
    private Rigidbody rb;
    public Transform carTransform;
    ///---PRIVATE---///
    void Start () 
    {
        rb = GetComponent<Rigidbody>();
        //agent = GetComponent<NavMeshAgent>();
    }
    
   void FixedUpdate () {
        float speedlInput  = Input.GetAxis("Vertical");
        //float turnInput = Input.GetAxis("Horizontal");

        carTransform.Translate(Vector3.forward * speedlInput * movementSpeed * Time.deltaTime);

        // NaveMesh code 
        // if (!agent.isOnNavMesh)
        // {
        //     Vector3 warpPosition = carTransform.position; 
        //     agent.Warp(warpPosition);
        // }
        // Transform target = carTransform;
        // target.position += Vector3.forward * speedlInput * movementSpeed * Time.deltaTime;
        //agent.transform.Translate(Vector3.forward * speedlInput * movementSpeed * Time.deltaTime);
        
        // Smooth turn 360 
        //carTransform.Rotate(0, turnInput * rotationSpeed * Time.deltaTime, 0, Space.Self);
    }
    void Update() {
        // Discrete turn l/r 
        if (Input.GetKeyDown("left"))
        {
            carTransform.Rotate(0, -90, 0, Space.Self);
        } 
        if (Input.GetKeyDown("right"))
        {
            carTransform.Rotate(0, 90, 0, Space.Self);
        }
    }
}