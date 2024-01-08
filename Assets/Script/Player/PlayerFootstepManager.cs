using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFootstepManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioSource footstepEmitter;

    private void Start()
    {
        footstepEmitter.volume = .9f;
    }

    private void OnTriggerEnter(Collider other)
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
            RandomizePitch();
            RandomizeVolume();
            footstepEmitter.PlayOneShot(clip, .1f);
        }
    }

    private void RandomizePitch()
    {
        footstepEmitter.pitch = Random.Range(footstepEmitter.pitch - .1f, footstepEmitter.pitch + .1f);
    }

    private void RandomizeVolume()
    {
        footstepEmitter.volume = Random.Range(footstepEmitter.volume - .1f, footstepEmitter.volume + 1f);
    }
}
