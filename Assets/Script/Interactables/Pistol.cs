using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

[RequireComponent(typeof(AudioSource))]

public class Pistol : MonoBehaviour, IInteractable
{
    [SerializeField] private Image iconBG;
    [SerializeField] private Image icon;
    [SerializeField] private Image redFilter;
    [SerializeField] private TextMeshProUGUI ammoText;

    [SerializeField] private AudioClip pickup;
    private AudioSource audioSource;

    [Inject] PlayerAmmoManager ammoManager;
    [Inject] ChooseWeapon chooseWeapon;
    [Inject] PlayerAudioManager audioManager;


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
            if (!ammoManager.CanAcceptAmmo() && PlayerInteract.hasPrimary)
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
        if(!PlayerInteract.hasPrimary)
        {
            PlayerInteract.hasPrimary = true;
            chooseWeapon.SelectPrimary();

            int ammo = int.Parse(ammoText.text);
            ammoManager.CalculateAmmoFromPickup(this.gameObject, ammo);

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            PlayerInteract.playerPistolsPickedUp++;
#endif
        }
        else
        {
            int ammo = int.Parse(ammoText.text);
            PlayInteractionSound();

            ammoManager.CalculateAmmoFromPickup(this.gameObject, ammo);
        }      
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        PlayerInteract.playerAmmoClipCount++;
#endif
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
            ammoText.DOFade(alpha, .1f);
            ActivateRedFilter(false);
        }
    }
}
