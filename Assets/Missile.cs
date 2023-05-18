using UnityEngine;

public class Missile : MonoBehaviour
{
    public float maxSpeed;
    public float speed;
    public float lifetime;
    public float explosionDamage;
    public Vector3 target;
    public GameObject explosionPrefab;
    public GameObject owner;
    //public float speed;

    private Rigidbody rb;

    void Start()
    {
        // Get the rigidbody component of the bullet object
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, lifetime);
    }
    private void Update()
    {
        if (target == null)
            return;

        //Vector3 direction = (target - transform.position).normalized;

        // Apply force to the missile's Rigidbody to move it towards the target
        speed = Mathf.Lerp(speed, maxSpeed, 0.01f);
        rb.velocity = transform.forward * speed;

        // Rotate the missile to look at the target
        //transform.rotation = Quaternion.LookRotation(transform.forward);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner)
        {
            // create explosion
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            ExplosionDamage script = explosion.GetComponent<ExplosionDamage>();
            script.damage = explosionDamage;

            Destroy(gameObject);
        }
    }
}
