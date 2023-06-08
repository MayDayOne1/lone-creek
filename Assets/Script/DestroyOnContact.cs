using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    public float ThrowableDamage = .1f;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
            if(collision.gameObject.tag == "Enemy")
            {
                // Debug.Log("Enemy hit with throwable");
                collision.gameObject.GetComponent<AI>().TakeDamage(ThrowableDamage);
            }
            return;
        }
    }
}
