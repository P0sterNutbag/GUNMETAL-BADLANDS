using UnityEngine;

public class scrBullet : MonoBehaviour
{
    public float lifetime = 2f;
    public int damage = 10;
    //public float speed;

    private Rigidbody2D rb;

    void Start()
    {
        // Get the rigidbody component of the bullet object
        rb = GetComponent<Rigidbody2D>();

        // Set the velocity of the bullet based on its forward direction and speed
        //rb.velocity = transform.forward * speed;

        // Destroy the bullet after its lifetime has expired
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
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
