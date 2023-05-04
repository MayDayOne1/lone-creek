using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{ 
    List<Collider> objectsTriggered = new List<Collider>();
    public GameObject Throwable;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Throwable")
        {
            // Debug.Log("I'm throwable!");
        }
        objectsTriggered.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        objectsTriggered.Remove(other);
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
            obj.GetComponent<PickupThrowable>().Interact();
            objectsTriggered.Remove(obj);
        }
    }

    
}
