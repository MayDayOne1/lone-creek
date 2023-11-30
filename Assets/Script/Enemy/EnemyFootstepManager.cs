using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootstepManager : MonoBehaviour
{
    [SerializeField] Transform footstepEmitter;
    [SerializeField] AudioClip[] footsteps;

    private AudioClip SelectRandomFootstep()
    {
        return footsteps[Random.Range(0, footsteps.Length)];
    }

#pragma warning disable IDE0051 // Remove unused private members
    private void Step()
#pragma warning restore IDE0051
    {
        AudioClip clip = SelectRandomFootstep();
        AudioSource.PlayClipAtPoint(clip, footstepEmitter.position);
    }
}
