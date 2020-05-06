using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    private NPCMovement movementFunctions;
    public string sequence = "a..p..s..";
    // Start is called before the first frame update
    private enum MoveState { None, Coasting, Ramming, Blocking };
    private MoveState currentMoveState = MoveState.Coasting;
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
                        StopAllCoroutines();
                        StartCoroutine(movementFunctions.speedUp());
                        break;
                    case 'p':
                        StopAllCoroutines();
                        StartCoroutine(movementFunctions.suddenStop());
                        break;
                    case 's':
                        StopAllCoroutines();
                        StartCoroutine(movementFunctions.slowDown());
                        break;
                    case 'r':
                        StopAllCoroutines();
                        StartCoroutine(movementFunctions.SwitchLaneRight(true, movementFunctions.movementSpeed));
                        break;
                    case 'l':
                        StopAllCoroutines();
                        StartCoroutine(movementFunctions.SwitchLaneRight(false, movementFunctions.movementSpeed));
                        break;
                    default:
                        yield return new WaitForSeconds(System.Convert.ToSingle(command));
                        break;
                }
            }
        }
    }
}
