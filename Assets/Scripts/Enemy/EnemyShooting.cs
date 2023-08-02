using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletForce;
    public float damage;

    public void ShootPlayer(GameObject targetPlayer)
    {
        //print("Shoot");
        CalcWhereToShoot();
        ShootProjectile(targetPlayer);
        // Debug.Log("bang");
        // Steal code from player, calc how to hit enemy
    }

    private Vector3 CalcWhereToShoot()
    {

        return new Vector3(0,0,0);
    }

    private void ShootProjectile(GameObject targetPlayer)
    {

        Vector3 towardsPlayer = (targetPlayer.transform.position - transform.position).normalized;
        // Create a new bullet object at the fire point
        GameObject projectileBullet = Instantiate(bulletPrefab, transform.position + (towardsPlayer * 2), transform.rotation);
        //projectileBullet.GetComponent<Bullet>().owner = player.gameObject;
        //projectileBullet.GetComponent<Bullet>().damage = damage;
        // Get the rigidbody component of the bullet object and apply a force to it to shoot it
        Rigidbody rb = projectileBullet.GetComponent<Rigidbody>();
        rb.AddForce((towardsPlayer) * bulletForce, ForceMode.Impulse);
        BulletProjectile bulletscript = projectileBullet.GetComponent<BulletProjectile>();
        bulletscript.damage = damage;
        bulletscript.owner = gameObject.GetComponent<BoxCollider>();
    }
}
