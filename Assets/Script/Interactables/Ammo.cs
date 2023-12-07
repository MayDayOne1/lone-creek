using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MEC;

[RequireComponent(typeof(AudioSource))]

public class Ammo : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerInteract interact;
    [SerializeField] private PlayerAmmoManager ammoManager;
    [SerializeField] private PlayerAudioManager audioManager;
    [SerializeField] private Image iconBG;
    [SerializeField] private Image icon;
    [SerializeField] private Image redFilter;
    [SerializeField] private TextMeshProUGUI ammoText;

    [SerializeField] private AudioClip pickup;
    private AudioSource audioSource;
    private bool isPlayingSound = false;
    void Start()
    {
        SetIconVisibility(0f);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = pickup;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            SetIconVisibility(1f);
            if (!ammoManager.CanAcceptAmmo())
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
        SetIconVisibility(0f);
    }

    public void ActivateRedFilter(bool activate)
    {
        if (activate && isActiveAndEnabled)
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
        int ammo = int.Parse(ammoText.text);
        PlayInteractionSound();
        ammoManager.CalculateAmmoFromPickup(this.gameObject, ammo);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        PlayerInteract.playerAmmoClipCount++;
#endif
    }

    public void PlayInteractionSound()
    {
        audioManager.PlayInteractionSound(pickup);
    }

    IEnumerator<float> PlaySound()
    {
        if (!isPlayingSound)
        {
            audioSource.PlayOneShot(pickup);
            isPlayingSound = true;
            yield return Timing.WaitForSeconds(pickup.length);
        }
        isPlayingSound = false;
    }

    public void SetIconVisibility(float alpha)
    {
        if (isActiveAndEnabled)
        {
            iconBG.DOFade(alpha, .1f);
            icon.DOFade(alpha, .1f);
            ammoText.DOFade(alpha, .1f);
            ActivateRedFilter(false);
        }
    }
}
