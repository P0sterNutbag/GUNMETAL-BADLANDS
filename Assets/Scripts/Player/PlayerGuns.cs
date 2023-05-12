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
    public float rotationSpeed = 5f;
    public float camZoomAmount = 2f;
    public float camZoomSpd = 0.25f;
    public bool isHitscanL = false;
    public bool isHitscanR = false;
    public bool isAutofireL = true;
    public bool isAutofireR = true;

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
    private bool fireButtonL;
    private bool fireButtonR;
    private Vector3 aimPoint;

    public enum gunType
    { 
        hitscan,
        projectile,
        missile
    }
    public gunType gunTypeL = gunType.projectile;
    public gunType gunTypeR = gunType.hitscan;

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
        if (isAutofireL) { fireButtonL = Input.GetButton("Fire1"); }
        else { fireButtonL = Input.GetButtonDown("Fire1"); }
        if (isAutofireR) { fireButtonR = Input.GetButton("Fire2"); }
        else { fireButtonR = Input.GetButtonDown("Fire2"); }
        // state machine
        switch (playerState.currentState)
        {
            case PlayerMovement.State.Walk:
                if (fireButtonL && Time.time >= nextTimetoFireL)
                {
                    nextTimetoFireL = Time.time + 1f / fireRateL;
                    Shoot(gunTypeL,firePointL.transform.position,firePointL.rotation);
                    animator.SetTrigger("ShootL");
                }
                if (fireButtonR && Time.time >= nextTimetoFireR)
                {
                    nextTimetoFireR = Time.time + 1f / fireRateR;
                    Shoot(gunTypeR, firePointR.transform.position, firePointR.rotation);
                    animator.SetTrigger("ShootR");
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
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out cast, range))
        {
            aimPoint = cast.point;
            Vector3 dir = aimPoint - firePoint;
            aimDir = dir.normalized;
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

                // Get the rigidbody component of the bullet object and apply a force to it to shoot it
                Rigidbody rb = projectileBullet.GetComponent<Rigidbody>();
                rb.AddForce(aimDir * bulletForce, ForceMode.Impulse);
                break;
            case gunType.missile:
                // Create a new bullet object at the fire point
                GameObject missile = Instantiate(missilePrefab, firePoint, fireRotation);

                // set missile target position
                missile.GetComponent<Missile>().target = aimPoint;
                missile.GetComponent<Missile>().speed = missileForceStart;
                missile.GetComponent<Missile>().maxSpeed = missileForceMax;

                // randomize direction
                float randomRange = 10f;
                Vector3 missleDir = aimDir + new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));

                // Get the rigidbody component of the bullet object and apply a force to it to shoot it
                rb = missile.GetComponent<Rigidbody>();
                rb.AddForce(missleDir * missileForceStart, ForceMode.Impulse);
                break;
        }
    }

    private void LateUpdate()
    {
        transform.position = player.position+playerdif;
        Quaternion targetRotation = Quaternion.Euler(fpsCam.transform.eulerAngles.x, player.rotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    /*private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        //trail.transform.position = hit.point;
        //Instantiate(ImpactParticalSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }*/
}
