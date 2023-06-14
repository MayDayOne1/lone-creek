using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    private int collisionCount = 0;
    public float ThrowableDamage = .1f;
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.tag.Equals("Player"))
        {
            Destroy(gameObject);
            if(collision.gameObject.tag.Equals("Enemy"))
            {
                collisionCount++;
                // Debug.Log(collision.gameObject.name);
                // Debug.Log("Enemy hit with throwable");
                if(collisionCount > 0)
                    collision.gameObject.GetComponentInParent<AI>().TakeDamage(ThrowableDamage);
            }
        }
    }
}
