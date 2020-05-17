using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedalPress : MonoBehaviour
{
    [SerializeField] private Image brakePedal, accelPedal;
    [SerializeField] private PlayerControls controls;
    // Start is called before the first frame update
    void Start()
    {
        controls = GameObject.Find("Player").GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        accelPedal.enabled = true;
        brakePedal.enabled = true;
        if (controls.isBraking())
        {
            brakePedal.enabled = false;
        }
        if (controls.isAcceling())
        {
            accelPedal.enabled = false;
        }
    }
}
