using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMovement : MonoBehaviour
{
    public AudioSource siren;
    private float movementSpeed = 0.1f;
    public float neutralSpeed = 0.1f;
    private float maxSpeed = 0.1f;
    private float strafeSpeed = 0.3f;
    private float acceleration = 0f;
    private float eyesight = 3;
    private Vector3 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        movementDirection = transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (SeesPlayer(transform.right))
        {
            Debug.Log("I c u");
            StartCoroutine(RamPlayer(transform.right));
        }
        else if (SeesPlayer(-transform.right))
        {
            StartCoroutine(RamPlayer(-transform.right));
        }
        else
        {
            movementSpeed = 0.1f;
            drive();
        }
    }

    void drive()
    {
        transform.position += movementDirection * movementSpeed;
    }

    bool SeesPlayer(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, eyesight);
        if(hit.collider != null) //doesnt actually check if it sees the player vs npc
        {
            Vector3 posRelativeToPlayer = hit.collider.gameObject.transform.InverseTransformPoint(transform.position);
            // Debug.Log(posRelativeToPlayer);
            if(posRelativeToPlayer.y > -0.1f && posRelativeToPlayer.y < 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator RamPlayer(Vector3 direction)
    {
        movementSpeed = 0.05f;
        yield return new WaitForSeconds(1f);
        transform.position += direction * strafeSpeed;
    }
}
