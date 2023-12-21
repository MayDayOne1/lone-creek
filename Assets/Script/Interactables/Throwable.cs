using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MEC;
using Zenject;

[RequireComponent(typeof(AudioSource))]

public class Throwable : MonoBehaviour, IInteractable
{
    [SerializeField] private Image iconBG;
    [SerializeField] private Image icon;
    [SerializeField] private Image redFilter;

    [SerializeField] private AudioClip pickup;
    private AudioSource audioSource;

    [Inject] private PlayerShootingManager shootingManager;
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
            if (PlayerParams.hasThrowable)
            {
                ActivateRedFilter(true);
            }
            else
            {
                ActivateRedFilter(false);
            }
        }
    }

    public void ActivateRedFilter(bool activate)
    {
        if(activate && redFilter.isActiveAndEnabled)
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
        if (PlayerParams.hasThrowable) return;
        else
        {
            PlayerParams.hasThrowable = true;
            if (!PlayerParams.hasPrimary) shootingManager.SelectThrowable();
            else
            {
                PlayInteractionSound();
            }
            Destroy(this.gameObject);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            PlayerParams.playerBottleCount++;
#endif
        }
    }

    public void PlayInteractionSound()
    {
        audioManager.PlayInteractionSound(pickup);
    }

    public void SetIconVisibility(float alpha)
    {
        if (gameObject != null)
        {
            if (iconBG != null) iconBG.DOFade(alpha, .1f);
            if (icon != null) icon.DOFade(alpha, .1f);
            ActivateRedFilter(false);
        }
    }
}
