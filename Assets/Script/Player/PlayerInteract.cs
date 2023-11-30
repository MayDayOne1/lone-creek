using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;

public class PlayerInteract : MonoBehaviour
{
    private List<IInteractable> interactablesTriggered = new();

    public static bool hasThrowable = false;
    public static bool hasPrimary = false;

    public static bool savedThrowable;
    public static bool savedPrimary;

    public GameObject Throwable;
    public GameObject Pistol;
    public AudioSource audioSource;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    public static int playerBottleCount = 0;
    public static int playerAmmoClipCount = 0;
    public static int playerPistolsPickedUp = 0;
#endif

    private void Start()
    {
        audioSource = Pistol.GetComponent<AudioSource>();

        Throwable.SetActive(false);
        Pistol.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            interactablesTriggered.Add(other.GetComponent<IInteractable>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<IInteractable>() != null)
        {
            interactablesTriggered.Remove(other.GetComponent<IInteractable>());
        }
    }

    public void Interact()
    { 
        if(interactablesTriggered.Count > 0)
        {
            foreach (IInteractable interactable in interactablesTriggered)
            {
                interactable.Interact();
            }
            interactablesTriggered.Clear();
        }
    }
}