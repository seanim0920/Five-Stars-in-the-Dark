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

                switch (command)
                {
                    case 'a':
                        movementFunctions.speedUp();
                        break;
                    case 'p':
                        movementFunctions.suddenStop();
                        break;
                    case 's':
                        movementFunctions.slowDown();
                        break;
                    case 'r':
                        movementFunctions.SwitchLaneRight(true, movementFunctions.movementSpeed);
                        break;
                    case 'l':
                        movementFunctions.SwitchLaneRight(false, movementFunctions.movementSpeed);
                        break;
                    default:
                        yield return new WaitForSeconds(System.Convert.ToSingle(command));
                        break;
                }
            }
        }
    }
}
