using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{ 
    private Collider OnTriggerEnter(Collider other)
    {
        if (other.tag == "Throwable")
        {
            Debug.Log("I'm throwable!");
            other.GetComponent<PickupThrowable>().Interact();
        }
        Collider result = other;
        return result;
    }
    public void Interact()
    { 
        Debug.Log("interact");
    }

    
}
