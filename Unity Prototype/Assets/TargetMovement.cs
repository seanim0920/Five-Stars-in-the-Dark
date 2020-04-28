using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    private NPCMovement movementFunctions;
    public string sequence;
    // Start is called before the first frame update
    void Start()
    {
        movementFunctions = GetComponent<NPCMovement>();
        StartCoroutine(movementPattern());
    }

    IEnumerator movementPattern()
    {
        while (true)
        {
            for (int i = 0; i < sequence.Length; i++)
            {
                char command = sequence[i];

                //switch (command)
                //{
                //    case 'c':
                //        movementFunctions.coast();
                //        break;
                //    case 'a':
                //        movementFunctions.speedUp();
                //        break;
                //    case 'p':
                //        movementFunctions.completeStop();
                //        break;
                //    case 's':
                //        movementFunctions.slowDown();
                //        break;
                //    case 'r':
                //        movementFunctions.changeLane(true);
                //        break;
                //    case 'l':
                //        movementFunctions.changeLane(false);
                //        break;
                //    default:
                //        yield return new WaitForSeconds(System.Convert.ToSingle(command));
                //        break;
                //}
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (SeesObstacle(transform.right) || SeesObstacle(-transform.right))
        //{
        //    movementSpeed += acceleration;
        //    //engineSound.pitch += acceleration / neutralSpeed;
        //}
    }

    IEnumerator SwitchLanes()
    {
        //randomly switch to the right or left lane
        while (true)
        {
            //how will the car figure out where to go? just check to the right or left to see if there is open space/curb
            yield return new WaitForSeconds(Random.Range(1.0f, 25.0f));

            Vector3 direction = transform.right;
            if (Random.Range(0, 1) == 0)
            {
                direction *= -1;
            }

            //2 is the size of a lane
            Vector3 goal = transform.position + direction * 2;
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, eyesight);
            //if (!SeesObstacle(direction) && hit.collider != null && hit.collider.gameObject.tag != "Guardrail")
            {
                while ((goal - transform.position).magnitude > 0.02f)
                {
                    //0.02f is movement speed while strafing
                    transform.position += (goal - transform.position).normalized * 0.02f;
                    yield return null;
                }
            }
        }
    }
}
