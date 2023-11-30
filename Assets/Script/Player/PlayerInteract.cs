using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;

public class PlayerInteract : MonoBehaviour
{
    private List<GameObject> ObjectsTriggered = new();

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
            ObjectsTriggered.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<IInteractable>() != null)
        {
            ObjectsTriggered.Remove(other.gameObject);
        }
    }
    private GameObject ChooseInteractiveObject()
    {
        float dist;
        float minDist = float.MaxValue;
        int resultingIndex = 0;
        if (ObjectsTriggered.Count > 0)
        {
            for (int i = 0; i < ObjectsTriggered.Count; i++)
            {
                dist = Vector3.Distance(ObjectsTriggered[i].transform.position, transform.position);
                if (dist < minDist)
                {
                    resultingIndex = i;
                    minDist = dist;
                }
            }
            return ObjectsTriggered[resultingIndex];
        } else return null;
    }

    public void Interact()
    { 
        if(ObjectsTriggered.Count > 0)
        {
            GameObject obj = ChooseInteractiveObject();
            obj.GetComponent<IInteractable>().Interact();
            ObjectsTriggered.Remove(obj); 
        }
    }
}