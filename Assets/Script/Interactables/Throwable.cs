using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Throwable : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerInteract interact;
    [SerializeField] private ChooseWeapon chooseWeapon;
    [SerializeField] private Image iconBG;
    [SerializeField] private Image icon;
    [SerializeField] private Image redFilter;

    void Start()
    { 
        SetIconVisibility(0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            SetIconVisibility(1f);
            if (PlayerInteract.hasThrowable)
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
        if(activate)
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
        if (PlayerInteract.hasThrowable) return;
        else
        {
            PlayerInteract.hasThrowable = true;
            if (!PlayerInteract.hasPrimary) chooseWeapon.SelectThrowable();
            Destroy(this.gameObject);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            PlayerInteract.playerBottleCount++;
#endif
        }
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
