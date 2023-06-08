using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{ 
    public GameObject Throwable;
    public GameObject Pistol;
    public AudioSource audioSource;

    private List<GameObject> objectsTriggered = new List<GameObject>();
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
            objectsTriggered.Add(other.gameObject);
            other.GetComponentInChildren<Canvas>().enabled = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        objectsTriggered.Remove(other.gameObject);
        other.GetComponentInChildren<Canvas>().enabled = false;
    }

    private GameObject ChooseInteractiveObject()
    {
        float dist;
        float minDist = float.MaxValue;
        int resultingIndex = 0;
        // Debug.Log("interact");
        if (objectsTriggered.Count > 0)
        {
            for (int i = 0; i < objectsTriggered.Count; i++)
            {
                dist = Vector3.Distance(objectsTriggered[i].transform.position, transform.position);
                if (dist < minDist)
                {
                    resultingIndex = i;
                    minDist = dist;
                }
            }
            return objectsTriggered[resultingIndex];
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
        }
    }
    private void PickupPistol(GameObject obj)
    {
        if (chooseWeapon.hasPistol == false)
        {
            chooseWeapon.hasPistol = true;
            chooseWeapon.SelectPrimary();
            playerShootingManager.SetAmmo(24);
        }
    }
    private void PickupAmmo(GameObject obj)
    {
        throw new NotImplementedException();
    }
    public void Interact()
    { 
        if(objectsTriggered.Count > 0)
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
            Destroy(obj);
            objectsTriggered.Remove(obj); 
        }
    }
}