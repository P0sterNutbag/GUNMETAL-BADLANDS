using UnityEngine;

public class Missile : MonoBehaviour
{
    public float lifetime;
    public GameObject explosionPrefab;
    public GameObject owner;

    [HideInInspector]
    public float maxSpeed;
    public float speed;
    public float explosionDamage;
    public Vector3 moveDirection;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, lifetime);
    }
    private void Update()
    {
        speed = Mathf.Lerp(speed, maxSpeed, 0.01f);
        rb.velocity = moveDirection * speed; 
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
