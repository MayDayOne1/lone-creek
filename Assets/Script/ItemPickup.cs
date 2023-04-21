using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float pickupRadius = 1f; // promień, w którym gracz może podnieść przedmiot
    public LayerMask playerLayer; // warstwa, na której znajduje się gracz
    public string playerTag = "Player"; // tag, którym oznaczony jest gracz

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius, playerLayer); // pobierz kolizje w promieniu pickupRadius z warstwą playerLayer

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(playerTag)) // sprawdź, czy tag obiektu jest taki sam jak playerTag
            {
                Debug.Log("Gracz jest w promieniu przedmiotu, który może podnieść."); // jeśli tak, wyświetl komunikat w konsoli
            }
        }
    }
}
