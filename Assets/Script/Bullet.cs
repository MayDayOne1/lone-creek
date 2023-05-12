using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        } else if (collision.gameObject.tag == "Player")
        {
            collision.transform.position = new Vector3(-33.4971771f, 0.000101089478f, -37.1840973f);
        } else
        {
            Destroy(gameObject);
        }

    }
}
