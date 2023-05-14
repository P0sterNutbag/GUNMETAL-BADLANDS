using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuns : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRateL = 15f;
    public float fireRateR = 15f;
    public float bulletForce = 20f;
    public float missileForceStart = 5f;
    public float missileForceMax = 20f;
    public int missileDamage = 5;
    public float rotationSpeed = 5f;
    public float camZoomAmount = 2f;
    public float camZoomSpd = 0.25f;
    public float bulletsPerShotL = 3f;
    public float bulletsDelayL = 0.5f;
    public float bulletsPerShotR = 3f;
    public float bulletsDelayR = 0.5f;

    public Camera fpsCam;
    public Animator animator;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public Transform firePointL;
    public Transform firePointR;
    public GameObject bulletImpact;
    public PlayerMovement playerState;
    private Transform player;
    public TrailRenderer bulletTrail;
    private float nextTimetoFireR = 0f;
    private float nextTimetoFireL = 0f;
    private float nextTimetoBulletR = 0f;
    private float nextTimetoBulletL = 0f;
    private float bulletCounterR = 0f;
    private float bulletCounterL = 0f;
    private bool fireButtonL;
    private bool fireButtonR;
    private bool isShootingL;
    private bool isShootingR;
    //private Vector3 aimPoint;

    public enum gunType
    { 
        hitscan,
        projectile,
        missile
    }
    public gunType gunTypeL = gunType.projectile;
    public gunType gunTypeR = gunType.hitscan;

    public enum fireType
    {
        single,
        burst,
        auto
    }
    public fireType fireTypeL = fireType.single;
    public fireType fireTypeR = fireType.burst;

    Vector3 playerdif;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerdif = new Vector3(transform.position.x-player.position.x, transform.position.y-player.position.y, transform.position.z-player.position.z);   
    }

    // Update is called once per frame
    void Update()
    {
        // inputs
        if (fireTypeL == fireType.auto) { fireButtonL = Input.GetButton("Fire1"); }
        else { fireButtonL = Input.GetButtonDown("Fire1"); }
        if (fireTypeR == fireType.auto) { fireButtonR = Input.GetButton("Fire2"); }
        else { fireButtonR = Input.GetButtonDown("Fire2"); }
        // state machine
        switch (playerState.currentState)
        {
            case PlayerMovement.State.Walk:
                if (fireButtonL && Time.time >= nextTimetoFireL)
                {
                    nextTimetoFireL = Time.time + 1f / fireRateL;
                    if (fireTypeL == fireType.burst)
                    {
                        bulletCounterL = 0f;
                    }
                    else 
                    {
                        Shoot(gunTypeL, firePointL.transform.position, firePointL.rotation);
                    }
                    animator.SetTrigger("ShootL");
                }
                if (fireButtonR && Time.time >= nextTimetoFireR)
                {
                    nextTimetoFireR = Time.time + 1f / fireRateR;
                    if (fireTypeR == fireType.burst)
                    {
                        bulletCounterR = 0f;
                    }
                    else
                    {
                        Shoot(gunTypeR, firePointR.transform.position, firePointR.rotation);
                    }
                    animator.SetTrigger("ShootR");
                }
                // actually fire bullets from left gun
                if (bulletCounterL < bulletsPerShotL)
                {
                    if (Time.time >= nextTimetoBulletL)
                    {
                        Shoot(gunTypeL, firePointL.transform.position, firePointL.rotation);
                        bulletCounterL++;
                        nextTimetoBulletL = Time.time + bulletsDelayL;
                    }
                }
                // actually fire bullets from right gun
                if (bulletCounterR < bulletsPerShotR)
                {
                    if (Time.time >= nextTimetoBulletR)
                    {
                        Shoot(gunTypeR, firePointR.transform.position, firePointR.rotation);
                        bulletCounterR++;
                        nextTimetoBulletR = Time.time + bulletsDelayR;
                    }
                }
                break;
        }

        // zoom camera in and out
    }

    void Shoot(gunType myGunType, Vector3 firePoint, Quaternion fireRotation)
    {
        // get direction towards camera
        Vector3 aimDir;
        RaycastHit cast;
        if (Physics.Raycast(fpsCam.transform.position + fpsCam.transform.forward*1.25f, fpsCam.transform.forward, out cast, range))
        {
            Vector3 aimPoint = cast.point;
            Vector3 dir = aimPoint - firePoint;
            aimDir = dir.normalized;
            Debug.Log(cast.collider.gameObject.name);
        }
        else
        {
            aimDir = transform.forward;
        }

        switch (myGunType)
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

    private void LateUpdate()
    {
        transform.position = player.position+playerdif;
        Quaternion targetRotation = Quaternion.Euler(fpsCam.transform.eulerAngles.x, player.rotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

}
