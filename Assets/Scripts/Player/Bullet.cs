using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f;
    public float damage;
    public GameObject owner;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner)
        {
            // If the bullet collides with an object that has a Health component, damage it
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // Destroy the bullet on impact
            Destroy(gameObject);
        }
    }
    
}
