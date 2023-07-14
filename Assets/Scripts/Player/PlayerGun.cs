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
    //[Header("Gun Type")]
    //public gunType stats.myGunType = gunType.projectile;
    public enum fireType
    {
        single,
        burst,
        auto
    }
    //public fireType stats.myFireType = fireType.single;

    #region STATS ARE NOW IN THE GUNSTAT OBJECTS
    [Header("Generals Values")]
    /*public float damage = 10f;
    public float aimVarianceMin = 0.5f;
    public float aimVarianceMax = 3f;
    public float aimVarianceSpeed = 1f;
    public float aimVarianceCooldown = 1.25f;
    public float bulletForce = 150f;
    public float stats.impactForce = 30f;
    public float stats.fireRate = 15f;
    public float stats.ammo;

    [Header("Missile Values")]
    public float stats.missileForceStart = 5f;
    public float stats.missileForceMax = 20f;

    [Header("Burstfire Values")]
    public float stats.bulletsPerShot = 3f;
    public float stats.bulletsDelay = 0.5f;

    [Header("Chargeshot Values")]
    public float stats.chargeTimerMax;*/
    #endregion

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
        chargeTimer = stats.chargeTimerMax;
        aimVariance = stats.aimVarianceMin;
    }
    // Update is called once per frame
    void Update()
    {
        // inputs
        if (stats.myFireType == fireType.auto || stats.myGunType == gunType.charge) { fireButton = Input.GetButton(firebutton); }
        else { fireButton = Input.GetButtonDown(firebutton); }
        fireButtonDown = Input.GetButton(firebutton);

        // shoot
        if (fireButton && Time.time >= nextTimetoFire)
        {
            if (stats.ammo > 0)
            {
                stats.ammo--;
                nextTimetoFire = Time.time + 1f / stats.fireRate;
                if (stats.myGunType == gunType.charge)
                {
                    if (chargeTimer >= stats.chargeTimerMax)
                    {
                        Shoot(stats.myGunType, firePoint.transform.position, firePoint.rotation);
                        chargeTimer = 0;
                    }
                    else
                    {
                        chargeTimer += Time.deltaTime;
                        chargebar.GetComponent<Healthbar>().SetHealth(chargeTimer, stats.chargeTimerMax);
                    }
                }
                else
                { 
                    if (stats.myFireType == fireType.burst)
                    {
                        bulletCounter = 0f;
                    }
                    else
                    {
                        Shoot(stats.myGunType, firePoint.transform.position, firePoint.rotation);
                    }
                }
            }
        }
        else if (stats.myGunType == gunType.charge)
        {
            chargeTimer = 0;
            chargebar.GetComponent<Healthbar>().SetHealth(chargeTimer, stats.chargeTimerMax);
        }
        // burst fire
        if (bulletCounter < stats.bulletsPerShot)
        {
            if (Time.time >= nextTimetoBullet)
            {
                Shoot(stats.myGunType, firePoint.transform.position, firePoint.rotation);
                bulletCounter++;
                nextTimetoBullet = Time.time + stats.bulletsDelay;
            }
        }

        // aim variance
        if (fireButtonDown)
        {
            if (aimVariance < stats.aimVarianceMax)
            {
                aimVariance += stats.aimVarianceSpeed * Time.deltaTime;
            }
        } else if (aimVariance > stats.aimVarianceMin)
        {
            aimVariance = Mathf.Lerp(aimVariance, stats.aimVarianceMin, stats.aimVarianceCooldown * Time.deltaTime);
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
                        target.TakeDamage(stats.damage);
                    }

                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * stats.impactForce);
                    }
                }
                break;

            case gunType.projectile:
                // Create a new bullet object at the fire point
                GameObject projectileBullet = Instantiate(bulletPrefab, firePoint, fireRotation);
                projectileBullet.GetComponent<Bullet>().owner = player.gameObject;
                projectileBullet.GetComponent<Bullet>().damage = stats.damage;
                // Get the rigidbody component of the bullet object and apply a force to it to shoot it
                Rigidbody rb = projectileBullet.GetComponent<Rigidbody>();
                rb.AddForce(aimDir * stats.bulletForce, ForceMode.Impulse);
                break;

            case gunType.missile:
                // Create a new bullet object at the fire point
                GameObject missile = Instantiate(missilePrefab, firePoint, fireRotation);

                // set missile target position
                //missile.GetComponent<Missile>().target = aimPoint;
                Missile script = missile.GetComponent<Missile>();
                script.speed = stats.missileForce;
                script.explosionDamage = stats.damage;
                script.owner = player.gameObject;
                script.moveDirection = aimDir;

                // randomize direction
                //float randomRange = 10f;
                //Vector3 missleDir = aimDir + new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));

                // Get the rigidbody component of the bullet object and apply a force to it to shoot it
                rb = missile.GetComponent<Rigidbody>();
                rb.AddForce(aimDir * stats.missileForce, ForceMode.Impulse);
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
                        target.TakeDamage(stats.damage);
                    }

                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * stats.impactForce);
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
        if (stats.ammo >= clipMax)
        {
            stats.ammoInClip = clipMax;
            stats.ammo -= clipMax;
        }
        else if (stats.ammo > 0)
        {
            stats.ammoInClip = stats.ammo;
            stats.ammo = 0;
        }
    }*/
}
