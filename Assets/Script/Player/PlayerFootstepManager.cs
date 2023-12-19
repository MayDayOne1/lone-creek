using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepManager : MonoBehaviour
{
    [SerializeField] AudioClip[] footsteps;
    [SerializeField] AudioSource footstepEmitter;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ok");
        Step();    
    }

    private AudioClip SelectRandomFootstep()
    {
        return footsteps[Random.Range(0, footsteps.Length)];
    }

    private void Step()
    {
        AudioClip clip = SelectRandomFootstep();
        if(footstepEmitter != null )
        {
            if(!footstepEmitter.isPlaying)
            {
                footstepEmitter.clip = clip;
                footstepEmitter.Play();
            }
        }
        else
        {
            Debug.LogWarning("footstep emitter is null on " + gameObject.name);
        }
        
    }
}
