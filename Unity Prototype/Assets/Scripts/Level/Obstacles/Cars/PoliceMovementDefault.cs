using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMovementDefault : MonoBehaviour
{
    public AudioSource siren;
    public AudioSource drift;
    [SerializeField] private float strafeSpeed;
    // private float acceleration = 0f;
    [SerializeField] private float eyesight;
    private NPCMovement moveFunctions;

    //enums to keep track of movement states, to prevent duplicate running of states
    private enum MoveState { None, Coasting, Ramming, Blocking };
    private MoveState currentMoveState = MoveState.Coasting;

    // Start is called before the first frame update
    void Start()
    {
        moveFunctions = GetComponent<NPCMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //to be more inline with the narrative, maybe the police is just active for now until the player screws up.
        //this section checks both sides of the police car to find the player
        for (int i = 0; i < 2; i++)
        {
            Vector3 direction = transform.right;
            if (i == 0) direction = -transform.right;
            
            GameObject sideObstacle = moveFunctions.SeesObstacle(direction);
            if (sideObstacle != null && sideObstacle.tag == "Player" && currentMoveState != MoveState.Blocking)
            {
                print("sees player!");
                currentMoveState = MoveState.Blocking;
                StartCoroutine(blockPlayer(sideObstacle, direction));
            }
        }

        if (currentMoveState == MoveState.Coasting)
        {
            StartCoroutine(moveFunctions.Coast());
            currentMoveState = MoveState.None;
        }
    }
    IEnumerator blockPlayer(GameObject player, Vector3 direction)
    {
        print("trying to ram player");
        Vector3 posRelativeToPlayer = player.transform.InverseTransformPoint(transform.position);
        while (posRelativeToPlayer.y <= 1) //we will assume 50 is half the car's length
        {
            posRelativeToPlayer = player.transform.InverseTransformPoint(transform.position);
            print("catching up to player" + posRelativeToPlayer);
            yield return new WaitForFixedUpdate();
        }
        while (posRelativeToPlayer.y > 0)
        {
            posRelativeToPlayer = player.transform.InverseTransformPoint(transform.position);
            print("aligning with player");
            //if this car is getting ahead of the player, slow down until it's aligned with the player, then ram into them.
            moveFunctions.movementSpeed *= 0.98f;
            yield return new WaitForFixedUpdate();
        }

        // Debug.Log("I c u");
        StartCoroutine(moveFunctions.SwitchLaneRight(direction == transform.right, 0.5f));
        drift.Play();
        currentMoveState = MoveState.Coasting;
    }
}
