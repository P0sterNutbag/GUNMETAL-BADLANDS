using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun/Machinegun")]
public class MissileLauncher : GunStats
{
    [Header("Missile Values")]
    [Tooltip("Initial speed of missile")]
    public float missileForceStart = 5f;
    [Tooltip("Max speed of missile")]
    public float missileForceMax = 20f;
}
