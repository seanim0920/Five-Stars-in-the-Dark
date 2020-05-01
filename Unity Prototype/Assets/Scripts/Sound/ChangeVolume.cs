using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChangeVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer menuMaster;
    [SerializeField] private AudioMixer carDialogue;

    public void SetVolume(float newVolume)
    {
        menuMaster.SetFloat("Menu BGM", Mathf.Log10(newVolume) * 20);
        carDialogue.SetFloat("Dialogue Volume", Mathf.Log10(newVolume) * 20);
    }
}
