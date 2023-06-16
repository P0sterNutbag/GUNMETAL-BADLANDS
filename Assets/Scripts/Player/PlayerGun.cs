using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{

    public GunStats stats;

    public enum gunType
    {
        hitscan,
        projectile,
        missile,
        charge
    }
    [Header("Gun Type")]
    public gunType myGunType = gunType.projectile;
    public enum fireType
    {
        single,
        burst,
        auto
    }
    public fireType myFireType = fireType.single;

    [Header("Generals Values")]
    public float damage = 10f;
    public float aimVarianceMin = 0.5f;
    public float aimVarianceMax = 3f;
    public float aimVarianceSpeed = 1f;
    public float aimVarianceCooldown = 1.25f;
    public float bulletForce = 150f;
    public float impactForce = 30f;
    public float fireRate = 15f;
    public float ammo;

    [Header("Missile Values")]
    public float missileForceStart = 5f;
    public float missileForceMax = 20f;

    [Header("Burstfire Values")]
    public float bulletsPerShot = 3f;
    public float bulletsDelay = 0.5f;

    [Header("Chargeshot Values")]
    public float chargeTimerMax;
    
    [Header("Controls")]
    public string firebutton;

    [Header("References")]
    public Camera fpsCam;
    public Animator animator;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public Transform firePoint;
    public GameObject bulletImpact;
    public PlayerMovement playerState;
    private Transform player;
    public TrailRenderer bulletTrail;
    public GameObject chargebar;

    [Header("Private")]
    private float nextTimetoFire = 0f;
    private float nextTimetoBullet = 0f;
    private float bulletCounter = 0f;
    private float chargeTimer = 0;
    public float aimVariance;
    private bool fireButton;
    private bool fireButtonDown;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        chargeTimer = chargeTimerMax;
        aimVariance = aimVarianceMin;
        myGunType = stats.myGunType;
    }
    // Update is called once per frame
    void Update()
    {
        // inputs
        if (myFireType == fireType.auto || myGunType == gunType.charge) { fireButton = Input.GetButton(firebutton); }
        else { fireButton = Input.GetButtonDown(firebutton); }
        fireButtonDown = Input.GetButton(firebutton);

        // shoot
        if (fireButton && Time.time >= nextTimetoFire)
        {
            if (ammo > 0)
            {
                ammo--;
                nextTimetoFire = Time.time + 1f / fireRate;
                if (myGunType == gunType.charge)
                {
                    if (chargeTimer >= chargeTimerMax)
                    {
                        Shoot(myGunType, firePoint.transform.position, firePoint.rotation);
                        chargeTimer = 0;
                    }
                    else
                    {
                        chargeTimer += Time.deltaTime;
                        chargebar.GetComponent<Healthbar>().SetHealth(chargeTimer, chargeTimerMax);
                    }
                }
                else
                { 
                    if (myFireType == fireType.burst)
                    {
                        bulletCounter = 0f;
                    }
                    else
                    {
                        Shoot(myGunType, firePoint.transform.position, firePoint.rotation);
                    }
                }
            }
        }
        else if (myGunType == gunType.charge)
        {
            chargeTimer = 0;
            chargebar.GetComponent<Healthbar>().SetHealth(chargeTimer, chargeTimerMax);
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

        // aim variance
        if (fireButtonDown)
        {
            if (aimVariance < aimVarianceMax)
            {
                aimVariance += aimVarianceSpeed * Time.deltaTime;
            }
        } else if (aimVariance > aimVarianceMin)
        {
            aimVariance = Mathf.Lerp(aimVariance, aimVarianceMin, aimVarianceCooldown * Time.deltaTime);
        }

    }

    void Shoot(gunType type, Vector3 firePoint, Quaternion fireRotation)
    {
        // animate 
        animator.SetTrigger("Shoot");

        // get direction towards camera
        Vector3 aimDir;
        RaycastHit cast;
        if (Physics.Raycast(fpsCam.transform.position + fpsCam.transform.forward*1.25f, fpsCam.transform.forward, out cast))
        {
            Vector3 aimPoint = cast.point;
            Vector3 dir = aimPoint - firePoint;
            aimDir = dir.normalized;
        }
        else
        {
            aimDir = transform.forward;
        }
        // add aim variance
        Quaternion randomRotation = Quaternion.Euler(Random.Range(-aimVariance, aimVariance), Random.Range(-aimVariance, aimVariance), 0f);
        aimDir = randomRotation * aimDir;
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
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
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
                projectileBullet.GetComponent<Bullet>().damage = damage;
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
                script.explosionDamage = damage;
                script.owner = player.gameObject;
                script.moveDirection = aimDir;

                // randomize direction
                //float randomRange = 10f;
                //Vector3 missleDir = aimDir + new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));

                // Get the rigidbody component of the bullet object and apply a force to it to shoot it
                rb = missile.GetComponent<Rigidbody>();
                rb.AddForce(aimDir * missileForceStart, ForceMode.Impulse);
                break;
            case gunType.charge:
                // make bullet trail
                bullet = Instantiate(bulletTrail, firePoint, Quaternion.identity);
                bullet.startWidth = 1;
                bullet.endWidth = 1;
                bullet.AddPosition(firePoint);
                {
                    bullet.transform.position = firePoint + (aimDir * 200);
                }

                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
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
        }
    }

    public string GetRange()
    {
        if (Physics.Raycast(fpsCam.transform.position + fpsCam.transform.forward * 1.25f, fpsCam.transform.forward, out RaycastHit cast))
        {
            return System.Math.Round(cast.distance,2).ToString();
        }
        else
        {
            return "";
        }
    }

    /*private void Reload() 
    {
        if (ammo >= clipMax)
        {
            ammoInClip = clipMax;
            ammo -= clipMax;
        }
        else if (ammo > 0)
        {
            ammoInClip = ammo;
            ammo = 0;
        }
    }*/
}
