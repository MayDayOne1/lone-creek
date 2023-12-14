using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MEC;
using Zenject;

[RequireComponent(typeof(AudioSource))]

public class HealthKit : MonoBehaviour, IInteractable
{
    [SerializeField] private Image iconBG;
    [SerializeField] private Image icon;
    [SerializeField] private Image redFilter;

    [SerializeField] private AudioClip pickup;
    private AudioSource audioSource;

    [Inject] private PlayerController controller;
    [Inject] private PlayerAudioManager audioManager;

    void Start()
    {
        SetIconVisibility(0f);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = pickup;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            SetIconVisibility(1f);
            if (PlayerController.health >= 1f)
            {
                ActivateRedFilter(true);
            }
            else
            {
                ActivateRedFilter(false);
            }
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            SetIconVisibility(0f);
        }
    }

    public void ActivateRedFilter(bool activate)
    {
        if (activate && redFilter.isActiveAndEnabled)
        {
            redFilter.DOFade(.6f, .1f);
        }
        else
        {
            redFilter.DOFade(0f, .1f);
        }
    }

    public void Interact()
    {
        if(PlayerController.health < 1f)
        {
            controller.PlayerRestoreHealth(.25f);
            PlayInteractionSound();
            Destroy(this.gameObject);
        }
    }

    public void PlayInteractionSound()
    {
        audioManager.PlayInteractionSound(pickup);
    }

    public void SetIconVisibility(float alpha)
    {
        if (isActiveAndEnabled)
        {
            iconBG.DOFade(alpha, .1f);
            icon.DOFade(alpha, .1f);
            ActivateRedFilter(false);
        }
    }
}
