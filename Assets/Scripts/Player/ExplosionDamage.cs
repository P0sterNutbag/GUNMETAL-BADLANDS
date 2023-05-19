using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public float damage;
    public float damageRadius;

    private void Start()
    {
        //SphereCollider sphereCollider = GetComponent<SphereCollider>();
        //damageRadius = sphereCollider.radius;
        //Debug.Log(damage);
    }

    private void OnEnable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        foreach (Collider collider in colliders)
        {
            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                //Debug.Log(damage);
            }
        }
    }
}
