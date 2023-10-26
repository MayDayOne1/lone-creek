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
        if (!collision.gameObject.tag.Equals("Player"))
        {
            // AudioSource.PlayClipAtPoint(glassShatterSound, collision.transform.position);
            Destroy(gameObject);
            // Debug.Log(collision.gameObject.name);
            if(collision.gameObject.tag == "Enemy")
            {
                // Debug.Log("Enemy hit with throwable");
                collision.gameObject.GetComponentInParent<AI>().TakeDamage(ThrowableDamage);
            }
        }
    }
}
