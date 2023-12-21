using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFootstepManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioSource footstepEmitter;
    void OnTriggerEnter(Collider other)
    {
        Step();    
    }

    private AudioClip SelectRandomFootstep()
    {
        return footsteps[Random.Range(0, footsteps.Length)];
    }

    private void Step()
    {
        if(!footstepEmitter.isPlaying)
        {
            AudioClip clip = SelectRandomFootstep();
            footstepEmitter.clip = clip;
            footstepEmitter.PlayOneShot(clip, .1f);
        }
    }
}
