using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private readonly List<IInteractable> interactablesTriggered = new();
    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactablesTriggered.Add(interactable);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if(interactable != null)
        {
            interactable.SetIconVisibility(0f);
            interactablesTriggered.Remove(interactable);
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

    private void DebugListElements()
    {
        StringBuilder sb = new();
        foreach (IInteractable interactable in interactablesTriggered)
        {
            sb.AppendLine(interactable.ToString());
            Debug.Log(sb.ToString());
        }
    }
}