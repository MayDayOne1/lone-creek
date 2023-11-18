using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class PlayerInteract : MonoBehaviour
{
    private ChooseWeapon chooseWeapon;
    private PlayerAmmoManager ammoManager;
    private PlayerController playerController;
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
        chooseWeapon = GetComponent<ChooseWeapon>();
        ammoManager = GetComponent<PlayerAmmoManager>();
        playerController = GetComponent<PlayerController>();
        Throwable.SetActive(false);
        Pistol.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Items")
        {
            ObjectsTriggered.Add(other.gameObject);
            ItemIconSetter iis = other.gameObject.GetComponentInChildren<ItemIconSetter>();
            if(iis != null) iis.SetIconVisibility(1f);

            if (other.gameObject.CompareTag("Throwable"))
            {
                if(!hasThrowable) ActivateRedItemFilter(other.gameObject, false);
                else ActivateRedItemFilter(other.gameObject, true);
            }

            if(other.gameObject.CompareTag("Ammo") || other.gameObject.CompareTag("Pistol"))
            {
                if(ammoManager.CanAcceptAmmo()) ActivateRedItemFilter(other.gameObject, false);
                else ActivateRedItemFilter(other.gameObject, true);
            }

            if(other.gameObject.CompareTag("HealthKit"))
            {
                if(playerController.GetHealth() < 1f) ActivateRedItemFilter(other.gameObject, false);
                else ActivateRedItemFilter(other.gameObject, true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(LayerMask.LayerToName(other.gameObject.layer) == "Items")
        {
            ObjectsTriggered.Remove(other.gameObject);
            ItemIconSetter iis = other.gameObject.GetComponentInChildren<ItemIconSetter>();
            if (iis != null) iis.SetIconVisibility(0f);
        }
    }
    private void ActivateRedItemFilter(GameObject other, bool enable)
    {
        foreach (Image img in other.GetComponentsInChildren<Image>())
        {
            if (img.gameObject.CompareTag("RedFilter"))
            {
                // Debug.Log("Setting red filter...");
                if (enable) img.DOFade(.6f, .1f);
                return;
            }
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
    private void PickupThrowable(GameObject obj)
    {
        if (hasThrowable == false)
        {
            hasThrowable = true;
            chooseWeapon.SelectThrowable();
            Destroy(obj);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            playerBottleCount++;
#endif
        }
        else return;
    }
    private void PickupPistol(GameObject obj)
    {
        if (hasPrimary == false)
        {
            hasPrimary = true;
            chooseWeapon.SelectPrimary();
            Destroy(obj);
        }

        string ammoText = obj.GetComponentInChildren<TextMeshProUGUI>().text;
        int ammo = int.Parse(ammoText);
        ammoManager.CalculateAmmoFromPickup(obj, ammo);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        playerAmmoClipCount++;
        playerPistolsPickedUp++;
#endif
    }
    private void PickupAmmo(GameObject obj)
    {
        string ammoText = obj.GetComponentInChildren<TextMeshProUGUI>().text;
        int ammo = int.Parse(ammoText);
        
        ammoManager.CalculateAmmoFromPickup(obj, ammo);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        playerAmmoClipCount++;
#endif
    }
    private void PickupHealth(GameObject obj)
    {
        if(playerController.GetHealth() < 1f)
        {
            playerController.PlayerRestoreHealth(.25f);
            Destroy(obj);
        }
    }
    public void Interact()
    { 
        if(ObjectsTriggered.Count > 0)
        {
            GameObject obj = ChooseInteractiveObject();
            if(obj.CompareTag("Throwable"))
            {
                PickupThrowable(obj);
            }
            else if (obj.CompareTag("Pistol"))
            {
                PickupPistol(obj);
            }
            else if (obj.CompareTag("Ammo"))
            {
                PickupAmmo(obj);
            }
            else if (obj.CompareTag("HealthKit"))
            {
                PickupHealth(obj);
            }
            
            ObjectsTriggered.Remove(obj); 
        }
    }
}