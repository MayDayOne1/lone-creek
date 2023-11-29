using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class EnemyFootstepManager : MonoBehaviour
{
    [SerializeField] Transform footstepEmitter;
    [SerializeField] AudioClip[] footsteps;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
