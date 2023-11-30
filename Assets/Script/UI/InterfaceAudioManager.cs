using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]

public class InterfaceAudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip onHoverClip;
    [SerializeField] private AudioClip onClickClip;
    [SerializeField] private AudioClip onBackClip;
    [SerializeField] private AudioClip onGameStartClip;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHover()
    {
        audioSource.clip = onHoverClip;
        audioSource.Play();
    }

    public void PlayClick()
    {
        audioSource.clip = onClickClip;
        audioSource.Play();
    }

    public void PlayBack()
    {
        audioSource.clip = onBackClip;
        audioSource.Play();
    }

    public void TogglePlay(bool isToggled)
    {
        if(isToggled)
        {
            PlayClick();
        }
        else
        {
            PlayBack();
        }
    }    

    public void PlayGameStart()
    {
        audioSource.clip = onGameStartClip;
        audioSource.Play();
    }
}
