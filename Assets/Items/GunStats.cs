using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Equipment/Gun")]
public class GunStats : Equipment
{
    public PlayerGun.gunType myGunType;

    [Header("Generals Values")]
    public float damage = 10f;
    public float aimVarianceMin = 0.5f;
    public float aimVarianceMax = 3f;
    [Tooltip("Rate at which aim variance increases in steps")]
    public float aimVarianceSpeed = 1f;
    [Tooltip("Rate at which aim variance decreases in steps")]
    public float aimVarianceCooldown = 1.25f;
    [Tooltip("Bullet's forward speed at creation")]
    public float bulletForce = 150f;
    [Tooltip("Bullet's Impact force on physics objects")]
    public float impactForce = 30f;
    [Tooltip("Amount of bullets fired every second")]
    public float fireRate = 15f;
    public float ammo;

    [Header("Missile Values")]
    [Tooltip("Initial speed of missile")]
    public float missileForceStart = 5f;
    [Tooltip("Max speed of missile")]
    public float missileForceMax = 20f;

    [Header("Burstfire Values")]
    public float bulletsPerShot = 3f;
    [Tooltip("Delay between each fire in burst")]
    public float bulletsDelay = 0.5f;

    [Header("Chargeshot Values")]
    [Tooltip("Time for a charge shot to charge")]
    public float chargeTimerMax;
}
