using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepManager : MonoBehaviour
{
    [SerializeField] Transform footstepEmitter;
    [SerializeField] AudioClip[] footsteps;

    private AudioClip SelectRandomFootstep()
    {
        return footsteps[Random.Range(0, footsteps.Length)];
    }

    private void Step()
    {
        AudioClip clip = SelectRandomFootstep();
        AudioSource.PlayClipAtPoint(clip, footstepEmitter.position);
    }
}
