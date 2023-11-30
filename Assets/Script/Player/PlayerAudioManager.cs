using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip[] damages;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    public void PlayGameOverSound()
    {
        audioSource.clip = gameOver;
        audioSource.Play();
    }

    private AudioClip SelectRandomDamageClip()
    {
        return damages[Random.Range(0, damages.Length)];
    }

    public void PlayDamageSound()
    {
        AudioClip damage = SelectRandomDamageClip();
        audioSource.clip = damage;
        audioSource.Play();
    }
}
