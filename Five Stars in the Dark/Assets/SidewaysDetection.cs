using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewaysDetection : MonoBehaviour
{
    public AudioLowPassFilter filter;
    GameObject player;
    AudioSource tires;
    // Start is called before the first frame update
    void Start()
    {
        tires = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        Vector3 posRelativeToPlayer = player.transform.InverseTransformPoint(transform.position);
        tires.volume = (1 + posRelativeToPlayer.y)/2;
        filter.lowpassResonanceQ = ((1 + posRelativeToPlayer.y) / 2)*5;
        filter.cutoffFrequency = ((1 + posRelativeToPlayer.y) / 2) * 500 + 200;
    }
}
