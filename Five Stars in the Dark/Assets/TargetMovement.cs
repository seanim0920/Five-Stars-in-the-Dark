using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    private NPCMovement movementFunctions;
    public string sequence = "a.....p....c....r....l....";
    // Start is called before the first frame update
    private enum MoveState { None, Coasting, Ramming, Blocking };
    private MoveState currentMoveState = MoveState.Coasting;

    private IEnumerator stopCoroutine;
    private IEnumerator slowCoroutine;
    private IEnumerator coastCoroutine;
    private IEnumerator fastCoroutine;
    private IEnumerator rightCoroutine;
    private IEnumerator leftCoroutine;
    void Start()
    {
        makeCoroutines();
        StartCoroutine(movementPattern());
    }

    void makeCoroutines()
    {
        movementFunctions = GetComponent<NPCMovement>();
        stopCoroutine = movementFunctions.suddenStop();
        slowCoroutine = movementFunctions.slowDown();
        coastCoroutine = movementFunctions.Coast();
        fastCoroutine = movementFunctions.speedUp();
        rightCoroutine = movementFunctions.SwitchLaneRight(true, movementFunctions.minSpeed);
        leftCoroutine = movementFunctions.SwitchLaneRight(false, movementFunctions.minSpeed);
    }

    void resetMovement()
    {
        StopCoroutine(stopCoroutine);
        StopCoroutine(slowCoroutine);
        StopCoroutine(coastCoroutine);
        StopCoroutine(fastCoroutine);
        StopCoroutine(rightCoroutine);
        StopCoroutine(leftCoroutine);
        makeCoroutines();
    }

    IEnumerator movementPattern()
    {
        while (true)
        {
            print("trying to move target car");
            for (int i = 0; i < sequence.Length; i++)
            {
                char command = sequence[i];
                print(command);

                switch (command)
                {
                    case 'a':
                        resetMovement();
                        StartCoroutine(fastCoroutine);
                        break;
                    case 'p':
                        resetMovement();
                        StartCoroutine(stopCoroutine);
                        break;
                    case 's':
                        resetMovement();
                        StartCoroutine(slowCoroutine);
                        break;
                    case 'c':
                        resetMovement();
                        StartCoroutine(coastCoroutine);
                        break;
                    case 'r':
                        resetMovement();
                        StartCoroutine(rightCoroutine);
                        break;
                    case 'l':
                        resetMovement();
                        StartCoroutine(leftCoroutine);
                        break;
                    default:
                        //yield return new WaitForSeconds(System.Convert.ToSingle(command)); //if we wanted the time between moves to be variable
                        yield return new WaitForSeconds(1f);
                        break;
                }
            }
            resetMovement();
            print("finished target car sequence");
        }
    }
}