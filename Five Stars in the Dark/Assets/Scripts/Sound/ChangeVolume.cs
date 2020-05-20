using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(Slider))]
public class ChangeVolume : MonoBehaviour
{
    [SerializeField] private int volumeIndex;
    [SerializeField] private AudioMixer[] mixers;
    [SerializeField] private string[] exposedParameters;

    void Start()
    {
        gameObject.GetComponent<Slider>().value = SettingsManager.volumes[volumeIndex];
    }

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

    public void SetSliderValue(float newValue)
    {
        SettingsManager.volumes[volumeIndex] = newValue;
        // Debug.Log("Slider " + volumeIndex + ": " + SettingsManager.volumes[volumeIndex]);
    }
}