using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OverheadMovement : MonoBehaviour
{
    public CharacterController controller;
    public Legs stats;
    public Camera cam;

    [HideInInspector]
    public float moveDirection = 0f;
    public float currentSpeed = 0f;
    public float moveRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        // Get the mouse position
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition.z = transform.position.z; // Match the player's z-position

        // Rotate the player towards the mouse using LookAt
        transform.LookAt(mousePosition);

        // Get movement variables
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // determine speed
        if (vertical > 0)
        {
            currentSpeed += stats.accelerationSpd * Time.deltaTime;
        }
        else if (vertical < 0)
        {
            currentSpeed -= stats.decelerationSpd * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, -stats.speed, stats.speed);
        
        // determine direction
        moveDirection += horizontal * stats.turnSpeed * Time.deltaTime;
        Quaternion moveRotation = Quaternion.AngleAxis(moveDirection, Vector3.up);
        Vector3 movementVector = moveRotation * Vector3.forward * currentSpeed * Time.deltaTime;
        //if (isGrounded && !isBoosting) { movementVector.y = -4.5f; } // get rid of bumps down slopes
        
        // actually move
        controller.Move(movementVector);
    }
}
