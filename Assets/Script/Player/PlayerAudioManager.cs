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
        audioSource.volume = .9f;
    }

    public void PlayGameOverSound()
    {
        SetDefaultAudioSettings();
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
        RandomizePitch();
        RandomizeVolume();
        audioSource.Play();
    }

    public void PlayInteractionSound(AudioClip pickup)
    {
        SetDefaultAudioSettings();
        audioSource.clip = pickup;
        audioSource.Play();
    }

    private void RandomizePitch()
    {
        audioSource.pitch = Random.Range(audioSource.pitch - .1f, audioSource.pitch + .1f);
    }

    private void RandomizeVolume()
    {
        audioSource.volume = Random.Range(audioSource.volume - .1f, audioSource.volume + 1f);
    }

    private void SetDefaultAudioSettings()
    {
        SetDefaultPitch();
        SetDefaultVolume();
    }

    private void SetDefaultPitch()
    {
        audioSource.pitch = 1f;
    }

    private void SetDefaultVolume()
    {
        audioSource.volume = .9f;
    }
}
