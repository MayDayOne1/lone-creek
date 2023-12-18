using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepManager : MonoBehaviour
{
    [SerializeField] AudioClip[] footsteps;

    void OnCollisionEnter(Collision collision)
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
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
