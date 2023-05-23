using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public float damage;
    public GameObject target;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        /* if (other.gameObject != owner)
         {
             // If the bullet collides with an object that has a Health component, damage it
             EnemyHealth health = other.GetComponent<PlayerHealth>();
             if (health != null)
             {
                 health.TakeDamage(damage);
             }

             // Destroy the bullet on impact
             Destroy(gameObject);
         }*/
    }
}
