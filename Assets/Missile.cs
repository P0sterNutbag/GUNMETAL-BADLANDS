using UnityEngine;

public class Missile : MonoBehaviour
{
    public float maxSpeed = 20f;
    public float speed;
    public float lifetime = 2f;
    public int damage = 10;
    public Vector3 target;
    //public float speed;

    private Rigidbody rb;

    void Start()
    {
        // Get the rigidbody component of the bullet object
        rb = GetComponent<Rigidbody>();

        // Set the velocity of the bullet based on its forward direction and speed
        //rb.velocity = transform.forward * speed;

        // Destroy the bullet after its lifetime has expired
        Destroy(gameObject, lifetime);
    }
    private void Update()
    {
        if (target == null)
            return;

        Vector3 direction = (target - transform.position).normalized;

        // Apply force to the missile's Rigidbody to move it towards the target
        speed = Mathf.Lerp(speed, maxSpeed, 0.1f);
        rb.velocity = direction * speed;

        // Rotate the missile to look at the target
        transform.rotation = Quaternion.LookRotation(direction);
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
