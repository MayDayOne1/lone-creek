using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{ 
    public GameObject Throwable;
    public GameObject Pistol;
    public AudioSource audioSource;

    public List<GameObject> ObjectsTriggered = new List<GameObject>();
    private ChooseWeapon chooseWeapon;
    private PlayerShootingManager playerShootingManager;

    private void Start()
    {
        Throwable.SetActive(false);
        Pistol.SetActive(false);
        audioSource = Pistol.GetComponent<AudioSource>();
        chooseWeapon = GetComponent<ChooseWeapon>();
        playerShootingManager = GetComponent<PlayerShootingManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Items")
        {
            // Debug.Log("Triggered");
            ObjectsTriggered.Add(other.gameObject);
            other.GetComponentInChildren<Canvas>().enabled = true;

            if(other.gameObject.tag.Equals("Throwable") && chooseWeapon.hasThrowable)
            {
                Image[] images = other.GetComponentsInChildren<Image>();
                foreach(Image img in images)
                {
                    if (img.gameObject.tag.Equals("RedFilter"))
                    {
                        img.enabled = true;
                        return;
                    }
                }
            }

            if((other.gameObject.tag.Equals("Ammo") || other.gameObject.tag.Equals("Pistol")) &&
                (playerShootingManager.currentAmmo + playerShootingManager.currentClip > 31))
            {
                Image[] images = other.GetComponentsInChildren<Image>();
                foreach (Image img in images)
                {
                    if (img.gameObject.tag.Equals("RedFilter"))
                    {
                        img.enabled = true;
                        return;
                    }
                }
            }
            //foreach (var obj in ObjectsTriggered)
            //{
            //    Debug.Log(obj.name);
            //}
            //Debug.Log("ObjectsTriggered Count: " + ObjectsTriggered.Count);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        ObjectsTriggered.Remove(other.gameObject);
        other.GetComponentInChildren<Canvas>().enabled = false;

        if (other.gameObject.tag.Equals("Throwable") && chooseWeapon.hasThrowable)
        {
            Image[] images = other.GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                if (img.gameObject.tag.Equals("RedFilter"))
                {
                    img.enabled = false;
                    return;
                }
            }
        }

        if ((other.gameObject.tag.Equals("Ammo") || other.gameObject.tag.Equals("Pistol")) &&
            (playerShootingManager.currentAmmo + playerShootingManager.currentClip > 31))
        {
            Image[] images = other.GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                if (img.gameObject.tag.Equals("RedFilter"))
                {
                    img.enabled = false;
                    return;
                }
            }
        }

        // Debug.Log("Exit, ObjectsTriggered Count: " + ObjectsTriggered.Count);
    }

    private GameObject ChooseInteractiveObject()
    {
        float dist;
        float minDist = float.MaxValue;
        int resultingIndex = 0;
        // Debug.Log("interact");
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
        } else
        {
            return null;
        }
    }
    private void PickupThrowable(GameObject obj)
    {
        if (chooseWeapon.hasThrowable == false)
        {
            chooseWeapon.hasThrowable = true;
            chooseWeapon.SelectThrowable();
            Destroy(obj);
        }
        else return;
    }
    private void PickupPistol(GameObject obj)
    {
        if (chooseWeapon.hasPistol == false)
        {
            chooseWeapon.hasPistol = true;
            chooseWeapon.SelectPrimary();
            playerShootingManager.SetAmmo(24);
            Destroy(obj);
        }
    }
    private void PickupAmmo(GameObject obj)
    {
        if (!chooseWeapon.hasPistol) return;
        string ammoText = obj.GetComponentInChildren<TextMeshProUGUI>().text;
        int ammo = int.Parse(ammoText);
        
        int ammoDiff = playerShootingManager.maxAmmo - playerShootingManager.currentAmmo;
        if(ammo <= ammoDiff)
        {
            // Debug.Log("Ammo <= ammoDiff");
            ammo += playerShootingManager.currentAmmo;
            playerShootingManager.currentAmmo = ammo;
            playerShootingManager.TotalAmmoUI.text = ammo.ToString();
            Destroy(obj);
        } else if (ammo + playerShootingManager.currentAmmo > playerShootingManager.maxAmmo)
        {
            ammoDiff = ammo + playerShootingManager.currentAmmo - playerShootingManager.maxAmmo;
            playerShootingManager.currentAmmo = playerShootingManager.maxAmmo;
            playerShootingManager.TotalAmmoUI.text = playerShootingManager.maxAmmo.ToString();

            obj.GetComponentInChildren<TextMeshProUGUI>().text = (ammoDiff).ToString();

        } else
        {
            Debug.Log("Ammo > ammoDiff");
            ammoDiff += playerShootingManager.currentAmmo;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = (ammo - ammoDiff).ToString();
            playerShootingManager.TotalAmmoUI.text = ammoDiff.ToString();
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
            else if (obj.tag == "Pistol")
            {
                PickupPistol(obj);
            } else if (obj.tag == "Ammo")
            {
                PickupAmmo(obj);
            }
            
            ObjectsTriggered.Remove(obj); 
        }
    }
}