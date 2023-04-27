using UnityEngine;
using Zenject;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private PlayerInteract playerInteract;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
           if(playerInteract.)
        }
    }
}
