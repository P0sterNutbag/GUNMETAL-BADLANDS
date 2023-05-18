using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f;
    public float damage;
    public GameObject owner;

    private Vector3 positionLast;
    //public float speed;

    private Rigidbody rb;

    void Start()
    {
        positionLast = transform.position;
        // Get the rigidbody component of the bullet object
        rb = GetComponent<Rigidbody>();

        // Set the velocity of the bullet based on its forward direction and speed
        //rb.velocity = transform.forward * speed;

        // Destroy the bullet after its lifetime has expired
        Destroy(gameObject, lifetime);
    }
    /*
    private void FixedUpdate()
    {
        RaycastHit other;
        if (Physics.Raycast(transform.position, positionLast - transform.position, out other))
        {
            if (other.collider.isTrigger == false)
            {
                // If the bullet collides with an object that has a Health component, damage it
                GameObject enemy = other.collider.gameObject;
                if (enemy.CompareTag("Enemy"))
                {
                    EnemyHealth health = enemy.GetComponent<EnemyHealth>();
                    if (health != null)
                    {
                        health.TakeDamage(damage);

                        Debug.Log("now thats what i call a hit");
                    }
                }
                Destroy(gameObject);
            }
        }
    }

    private void LateUpdate()
    {
        positionLast = transform.position;
    }
    */
    
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
