using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootstepManager : MonoBehaviour
{
    [SerializeField] private AudioSource footstepEmitter;
    [SerializeField] private AudioClip[] footsteps;

    private AudioClip SelectRandomFootstep()
    {
        return footsteps[Random.Range(0, footsteps.Length)];
    }

#pragma warning disable IDE0051 // Remove unused private members
    private void Step()
#pragma warning restore IDE0051
    {
        AudioClip clip = SelectRandomFootstep();
        RandomizePitch();
        RandomizeVolume();
        footstepEmitter.clip = clip;
        footstepEmitter.PlayOneShot(clip);
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
