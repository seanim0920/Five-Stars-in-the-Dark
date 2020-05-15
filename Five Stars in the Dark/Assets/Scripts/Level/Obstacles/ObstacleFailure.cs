using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class ObstacleFailure : MonoBehaviour
{
    [SerializeField]
    protected AudioSource dialogueSource;
    protected AudioClip[] failureDialogues;
    protected int numDialogue;

    protected virtual void Start()
    {
        dialogueSource = GetComponent<AudioSource>();
        numDialogue = 0;
    }

    public virtual void playFailure(Vector3 point)
    {
        // AudioSource.PlayClipAtPoint(dialogueSource.clip, point);
        dialogueSource.Play();
    }
}