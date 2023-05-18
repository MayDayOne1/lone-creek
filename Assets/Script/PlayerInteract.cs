using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{ 
    List<Collider> objectsTriggered = new List<Collider>();
    public GameObject Throwable;
    public ChooseWeapon chooseWeapon;

    private void Start()
    {
        Throwable.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Items")
        {
            // Debug.Log("Triggered");
            objectsTriggered.Add(other);
            other.GetComponentInChildren<Canvas>().enabled = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        objectsTriggered.Remove(other);
        other.GetComponentInChildren<Canvas>().enabled = false;
    }

    private Collider ChooseInteractiveObject()
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

    public void Interact()
    { 
        // TO DO: choose component to get by tag

        if(objectsTriggered.Count > 0)
        {
            Collider obj = ChooseInteractiveObject();
            if(obj.tag == "Throwable")
            {
                obj.GetComponent<PickupThrowable>().Interact();
                Throwable.SetActive(true);
                chooseWeapon.weaponSelected = ChooseWeapon.WEAPONS.THROWABLE;
            }
            
            objectsTriggered.Remove(obj); 
        }
    }

    
}
