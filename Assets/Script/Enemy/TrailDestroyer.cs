using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailDestroyer : MonoBehaviour
{
    private const int PLAYER_LAYER = 6;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == PLAYER_LAYER)
        {
            Destroy(gameObject);
        }
    }
}
