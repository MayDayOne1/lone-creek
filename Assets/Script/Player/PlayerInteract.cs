using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{ 
    List<GameObject> objectsTriggered = new List<GameObject>();
    public GameObject Throwable;
    public GameObject Pistol;
    private ChooseWeapon chooseWeapon;
    public AudioSource audioSource;

    private void Start()
    {
        Throwable.SetActive(false);
        Pistol.SetActive(false);
        chooseWeapon = GetComponent<ChooseWeapon>();
        audioSource = Pistol.GetComponent<AudioSource>();
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

    public void Interact()
    { 
        // TO DO: choose component to get by tag

        if(objectsTriggered.Count > 0)
        {
            GameObject obj = ChooseInteractiveObject();
            if(obj.CompareTag("Throwable"))
            {
                if (chooseWeapon.hasThrowable == false)
                {
                    chooseWeapon.hasThrowable = true;
                    chooseWeapon.SelectThrowable();
                    Destroy(obj);
                }
            }
            else if (obj.tag == "Pistol")
            {
                if (chooseWeapon.hasPistol == false)
                {
                    chooseWeapon.hasPistol = true;
                    chooseWeapon.SelectPrimary();
                    Destroy(obj);
                }
            }
            
            objectsTriggered.Remove(obj); 
        }
    }

    
}
