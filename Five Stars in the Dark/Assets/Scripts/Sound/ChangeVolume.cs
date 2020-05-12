using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChangeVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer[] mixers;
    [SerializeField] private string[] exposedParameters;

    public void SetVolume(float newVolume)
    {
        foreach(AudioMixer am in mixers)
        {
            foreach(string s in exposedParameters)
            {
                am.SetFloat(s, Mathf.Log10(newVolume) * 20);
            }
        }
    }
}
