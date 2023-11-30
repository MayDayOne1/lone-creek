using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem.XR;

public class HealthKit : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerInteract interact;
    [SerializeField] private PlayerController controller;
    [SerializeField] private Image iconBG;
    [SerializeField] private Image icon;
    [SerializeField] private Image redFilter;
    void Start()
    {
        SetIconVisibility(0f);
    }

    void OnTriggerEnter(Collider other)
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
        if(PlayerController.health < 1f)
        {
            controller.PlayerRestoreHealth(.25f);
            Destroy(this.gameObject);
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
