using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Pistol : MonoBehaviour, IInteractable
{
    [SerializeField] PlayerAmmoManager ammoManager;
    [SerializeField] ChooseWeapon chooseWeapon;
    [SerializeField] private Image iconBG;
    [SerializeField] private Image icon;
    [SerializeField] private Image redFilter;
    [SerializeField] private TextMeshProUGUI ammoText;

    void Start()
    {
        SetIconVisibility(0f);
    }

    void OnTriggerEnter(Collider other)
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
    private void OnTriggerExit(Collider other)
    {
        SetIconVisibility(0f);
    }
    public void ActivateRedFilter(bool activate)
    {
        if (activate)
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
            if (!PlayerInteract.hasThrowable)
            {
                chooseWeapon.SelectPrimary();
                Destroy(this.gameObject);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
                PlayerInteract.playerPistolsPickedUp++;
#endif
            }
        }
        int ammo = int.Parse(ammoText.text);
        ammoManager.CalculateAmmoFromPickup(this.gameObject, ammo);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        PlayerInteract.playerAmmoClipCount++;
#endif
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
