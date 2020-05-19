using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayError : MonoBehaviour
{
    public static AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator PauseDialogueCoroutine(AudioClip passengerHurt)
    {
        Debug.Log("Pausing Dialogue");
        AudioSource dialogue = ConstructLevelFromMarkers.levelDialogue;
        //play a random hurtsound
        source.clip = passengerHurt;
        source.Play();

        //find the last silent section of audio and rewind to it, or if not just rewind back a set amount of time
        float maxRewindTime = 2; //in seconds
        int maxRewindTimeInSamples = (int)((int)(dialogue.clip.samples / dialogue.clip.length) * maxRewindTime);
        float[] samples = new float[maxRewindTimeInSamples * dialogue.clip.channels]; //array to be filled with samples from the audioclip

        int currentTimePosition = dialogue.timeSamples - maxRewindTimeInSamples; //by default
        dialogue.clip.GetData(samples, currentTimePosition);

        bool wasPlaying = dialogue.isPlaying;
        dialogue.Pause();

        int foundSilences = 0;
        for (int i = samples.Length; i-- > 0;)
        {
            if (Mathf.Abs(samples[i]) == 0f)
            {
                foundSilences++;
                if (foundSilences >= 2)
                {
                    print("found silences.");
                    currentTimePosition += (i / dialogue.clip.channels);
                    break;
                }
            }
            else
            {
                foundSilences = 0;
            }
        }

        //cutsceneScript.levelDialogue.time = cutsceneScript.currentDialogueStartTime;
        //wait for... idk 3 seconds?
        yield return new WaitForSeconds(passengerHurt.length + 1f);  //note: should play an "oof" first before the angry passenger dialogue
        //resume dialogue
        dialogue.timeSamples = currentTimePosition;
        if (wasPlaying)
            dialogue.Play();
        Debug.Log("Resuming Dialogue");
    }
}
