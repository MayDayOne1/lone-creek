using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class DestroyOnContact : MonoBehaviour
{
    public float ThrowableDamage = .1f;
    public AudioClip glassShatterSound;

    private void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(glassShatterSound, collision.transform.position);
        Destroy(gameObject);
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInParent<AI>().TakeDamage(ThrowableDamage);
        }
        
    }
}
