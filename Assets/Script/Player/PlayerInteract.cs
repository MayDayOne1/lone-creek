using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private ChooseWeapon chooseWeapon;
    private PlayerShootingManager shootingManager;
    private PlayerAmmoManager ammoManager;
    private List<GameObject> ObjectsTriggered = new List<GameObject>();

    public bool hasThrowable = false;
    public bool hasPrimary = false;

    public GameObject Throwable;
    public GameObject Pistol;
    public AudioSource audioSource;


    private void Start()
    {
        audioSource = Pistol.GetComponent<AudioSource>();
        chooseWeapon = GetComponent<ChooseWeapon>();
        shootingManager = GetComponent<PlayerShootingManager>();
        ammoManager = GetComponent<PlayerAmmoManager>();
        Throwable.SetActive(false);
        Pistol.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Items")
        {
            ObjectsTriggered.Add(other.gameObject);
            other.GetComponentInChildren<Canvas>().enabled = true;

            if(other.gameObject.tag.Equals("Throwable") && hasThrowable)
            {
                RedFilterManager(other.gameObject, true);
            }

            if((other.gameObject.tag.Equals("Ammo") || other.gameObject.tag.Equals("Pistol")) &&
                !ammoManager.CanAcceptAmmo())
            {
                RedFilterManager(other.gameObject, true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(LayerMask.LayerToName(other.gameObject.layer) == "Items")
        {
            ObjectsTriggered.Remove(other.gameObject);
            other.GetComponentInChildren<Canvas>().enabled = false;

            if (other.gameObject.tag.Equals("Throwable") && hasThrowable)
            {
                RedFilterManager(other.gameObject, false);
            }

            if ((other.gameObject.tag.Equals("Ammo") || other.gameObject.tag.Equals("Pistol")) &&
                ammoManager.CanAcceptAmmo())
            {
                RedFilterManager(other.gameObject, false);
            }
        }
    }
    private void RedFilterManager(GameObject other, bool enable)
    {
        Image[] images = other.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            if (img.gameObject.tag.Equals("RedFilter"))
            {
                img.enabled = enable;
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
        }
        else return;
    }
    private void PickupPistol(GameObject obj)
    {
        if (hasPrimary == false)
        {
            hasPrimary = true;
            chooseWeapon.SelectPrimary();
            string ammoText = obj.GetComponentInChildren<TextMeshProUGUI>().text;
            int ammo = int.Parse(ammoText);
            ammoManager.SetAmmo(ammo);
            Destroy(obj);
        }
    }
    private void PickupAmmo(GameObject obj)
    {
        if (!hasPrimary) return;
        string ammoText = obj.GetComponentInChildren<TextMeshProUGUI>().text;
        int ammo = int.Parse(ammoText);
        
        ammoManager.CalculateAmmoFromPickup(obj, ammo);
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
            else if (obj.tag == "Pistol")
            {
                PickupPistol(obj);
            }
            else if (obj.tag == "Ammo")
            {
                PickupAmmo(obj);
            }
            
            ObjectsTriggered.Remove(obj); 
        }
    }
}