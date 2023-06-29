using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Equipment/Legs")]
public class Legs : Equipment
{
    public float speed = 12f;
    public float accelerationSpd = 0.1f;
    public float decelerationSpd = 0.2f;
    public float turnSpeed = 180f; // for tank controls
    public float gravity = -9.81f;
    public bool isTankControls = true;
}
