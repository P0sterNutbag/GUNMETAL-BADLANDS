using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;
    public float bulletForce = 150f;
    public float missileForceStart = 5f;
    public float missileForceMax = 20f;
    public int missileDamage = 5;
    public float bulletsPerShot = 3f;
    public float bulletsDelay = 0.5f;
    public float ammo;
    public float clipMax;
    public string firebutton;

    public Camera fpsCam;
    public Animator animator;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public Transform firePoint;
    public GameObject bulletImpact;
    public PlayerMovement playerState;
    private Transform player;
    public TrailRenderer bulletTrail;
    private float nextTimetoFire = 0f;
    private float nextTimetoBullet = 0f;
    private float bulletCounter = 0f;
    private float ammoInClip;
    private bool fireButton;
    private bool reloadButton;

    public enum gunType
    { 
        hitscan,
        projectile,
        missile
    }
    public gunType myGunType = gunType.projectile;

    public enum fireType
    {
        single,
        burst,
        auto
    }
    public fireType myFireType = fireType.single;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        ammoInClip = clipMax;
    }
    // Update is called once per frame
    void Update()
    {
        // inputs
        if (myFireType == fireType.auto) { fireButton = Input.GetButton(firebutton); }
        else { fireButton = Input.GetButtonDown(firebutton); }
        reloadButton = Input.GetButtonDown("Reload");


        // state machine
        if (fireButton && Time.time >= nextTimetoFire)
        {

                nextTimetoFire = Time.time + 1f / fireRate;
                if (myFireType == fireType.burst)
                {
                    bulletCounter = 0f;
                }
                else
                {
                    Shoot(myGunType, firePoint.transform.position, firePoint.rotation);
                }
                ammoInClip--;

        }
        // burst fire
        if (bulletCounter < bulletsPerShot)
        {
            if (Time.time >= nextTimetoBullet)
            {
                Shoot(myGunType, firePoint.transform.position, firePoint.rotation);
                bulletCounter++;
                nextTimetoBullet = Time.time + bulletsDelay;
            }
        }

        // reload
        if (reloadButton)
        {
            Reload();
        }
    }

    void Shoot(gunType type, Vector3 firePoint, Quaternion fireRotation)
    {
        // animate 
        animator.SetTrigger("ShootL");
        Debug.Log(type);

        // get direction towards camera
        Vector3 aimDir;
        RaycastHit cast;
        if (Physics.Raycast(fpsCam.transform.position + fpsCam.transform.forward*1.25f, fpsCam.transform.forward, out cast, range))
        {
            Vector3 aimPoint = cast.point;
            Vector3 dir = aimPoint - firePoint;
            aimDir = dir.normalized;
        }
        else
        {
            aimDir = transform.forward;
        }

        switch (type)
        {
            case gunType.hitscan:
                // make bullet trail
                var bullet = Instantiate(bulletTrail, firePoint, Quaternion.identity);
                bullet.AddPosition(firePoint);
                {
                    bullet.transform.position = firePoint + (aimDir * 200);
                }

                RaycastHit hit;
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
                {
                    EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
                    Vector3 hitPosition = hit.point;

                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }

                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * impactForce);
                    }
                }
                break;
            case gunType.projectile:
                // Create a new bullet object at the fire point
                GameObject projectileBullet = Instantiate(bulletPrefab, firePoint, fireRotation);
                projectileBullet.GetComponent<Bullet>().owner = player.gameObject;
                // Get the rigidbody component of the bullet object and apply a force to it to shoot it
                Rigidbody rb = projectileBullet.GetComponent<Rigidbody>();
                rb.AddForce(aimDir * bulletForce, ForceMode.Impulse);
                break;
            case gunType.missile:
                // Create a new bullet object at the fire point
                GameObject missile = Instantiate(missilePrefab, firePoint, fireRotation);

                // set missile target position
                //missile.GetComponent<Missile>().target = aimPoint;
                Missile script = missile.GetComponent<Missile>();
                script.speed = missileForceStart;
                script.maxSpeed = missileForceMax;
                script.explosionDamage = missileDamage;
                script.owner = player.gameObject;

                // randomize direction
                //float randomRange = 10f;
                //Vector3 missleDir = aimDir + new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));

                // Get the rigidbody component of the bullet object and apply a force to it to shoot it
                rb = missile.GetComponent<Rigidbody>();
                rb.AddForce(aimDir * missileForceStart, ForceMode.Impulse);
                break;
        }
    }

    private void Reload() 
    {
        ammoInClip = clipMax;
    }
}
