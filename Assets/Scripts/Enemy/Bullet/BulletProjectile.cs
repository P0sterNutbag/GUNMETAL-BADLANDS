using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public GameObject target;

    [HideInInspector]
    public float damage;

    void OnTriggerEnter(Collider other)
    {
        // If the bullet collides with an object that has a Health component, damage it
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        // Destroy the bullet on impact
        Destroy(gameObject);
    }
}
