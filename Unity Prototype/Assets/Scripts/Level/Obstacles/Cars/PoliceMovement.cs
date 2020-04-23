using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMovement : NPCMovement
{
    public AudioSource siren;
    // private float movementSpeed;
    // [SerializeField] private float neutralSpeed;
    // private float maxSpeed = 0.1f;
    [SerializeField] private float strafeSpeed;
    // private float acceleration = 0f;
    [SerializeField] private float eyesight;
    private Vector3 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        movementDirection = transform.up;
        movementSpeed = 0.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if (SeesPlayer(transform.right))
        // {
        //     // Debug.Log("I c u");
        //     StartCoroutine(RamPlayer(transform.right));
        // }
        // else if (SeesPlayer(-transform.right))
        // {
        //     // Debug.Log("I c u");
        //     StartCoroutine(RamPlayer(-transform.right));
        // }
        // else
        // {
            // movementSpeed = neutralSpeed;
            if(movementSpeed < neutralSpeed)
            {
                movementSpeed += 0.05f;
            }
            drive();
        // }
    }

    void drive()
    {
        transform.position += movementDirection * movementSpeed;
    }

    bool SeesPlayer(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, eyesight);
        if(hit.collider != null && hit.collider.gameObject.tag == "Player") //doesnt actually check if it sees the player vs npc
        {
            Vector3 posRelativeToPlayer = hit.collider.gameObject.transform.InverseTransformPoint(transform.position);
            // Debug.Log(posRelativeToPlayer);
            if(posRelativeToPlayer.y > -50f && posRelativeToPlayer.y < 50f)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator RamPlayer(Vector3 direction)
    {
        movementSpeed = 1f;
        yield return new WaitForSeconds(1f);
        transform.position += direction * strafeSpeed;
    }
}
