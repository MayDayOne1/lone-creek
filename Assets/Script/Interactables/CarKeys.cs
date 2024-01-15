using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

[RequireComponent(typeof(AudioSource))]

public class CarKeys : MonoBehaviour, IInteractable
{
    [SerializeField] private Image iconBG;
    [SerializeField] private Image icon;
    [SerializeField] private Image redFilter;

    [SerializeField] private AudioClip pickup;
    private AudioSource audioSource;

    [Inject] private PlayerController controller;
    [Inject] private PlayerAudioManager audioManager;
    [Inject] private NotificationManager notificationManager;


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
        controller.hasKeys = true;
        notificationManager.CarKeysNotification();
        PlayInteractionSound();
        Destroy(gameObject);
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
