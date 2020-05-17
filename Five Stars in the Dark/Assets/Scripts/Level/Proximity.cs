using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Proximity : MonoBehaviour
{
    List<AudioSource> NPCAudios = new List<AudioSource>();
    // Start is called before the first frame update
    void Start()
    {
    }
    private void Update()
    {
        foreach (AudioSource NPCAudio in NPCAudios.ToArray())
        {
            
            if (NPCAudio == null)
                NPCAudios.Remove(NPCAudio);
            else
            {
                print("detecting any obstacles between player and npc... " + Physics2D.Linecast(NPCAudio.gameObject.transform.position, transform.parent.transform.position).transform.tag);
                //check if there's a clear line of sight between NPC and player
                if (Physics2D.Linecast(NPCAudio.gameObject.transform.position, transform.parent.transform.position).transform.tag != "Player")
                    NPCAudio.volume = 0;
                else
                {
                    Vector3 difference = (NPCAudio.gameObject.transform.position - transform.parent.transform.position);
                    float distance = difference.magnitude;
                    float eyesight = transform.localScale.y * transform.parent.transform.localScale.y;
                    NPCAudio.volume = Mathf.Pow(((-distance / (eyesight)) + 1), 2) * 1.1f;
                    Vector3 posRelativeToPlayer = transform.parent.transform.InverseTransformPoint(NPCAudio.gameObject.transform.position);
                    NPCAudio.panStereo = posRelativeToPlayer.x / (transform.localScale.x / 2);
                    NPCAudio.pitch = NPCAudio.volume * 3;
                }
            }
                
            
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //should be adjusted to detect the closest car to the player, if there are multiple cars in the zone
        if (col.gameObject.tag == "Car")
        {
            NPCAudios.Add(col.gameObject.transform.Find("ProximitySfx").GetComponent<AudioSource>());
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Car")
        {
            col.gameObject.transform.Find("ProximitySfx").GetComponent<AudioSource>().volume = 0;
            NPCAudios.Remove(col.gameObject.transform.Find("ProximitySfx").GetComponent<AudioSource>());
        }
    }
}
