
using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public void ActivateRedFilter(bool activate);
    public void SetIconVisibility(float alpha);
}
