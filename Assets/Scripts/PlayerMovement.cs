using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHiehgt = 3f;
    public float slideForce = 10f;
    public float maxSlideAngle = 90f;
    public float turnSpeed = 180f; // for tank controls
    public float groundDistance = 0.4f;
    public float boostSpeed = 0f;
    public float boostAcceleration = 0.02f;
    public float boostMax = 12f;

    public Transform groundCheck;
    public LayerMask groundMask;
    public bool isHolding = false;
    public bool isTankControls = false;
    private float moveDirection = 0;
    private Quaternion moveRotation;

    private bool isSliding = false;
    private bool isBoosting = false;
    private bool isBoostVertical = false;

    public enum State
    { 
        Walk,
        Dead
    }
    public State currentState = State.Walk;


    Vector3 velocity;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Walk:
                // check for ground
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                }

                // get move inputs
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                // determine boost direction
                if (Input.GetKeyDown("left shift"))
                {
                    if ((horizontal == 0 && vertical == 0)) //|| (!isGrounded))
                    {
                        isBoostVertical = true;
                    }
                    else
                    {
                        isBoostVertical = false;
                    }
                }

                // boost
                if (Input.GetKey("left shift"))
                {
                    boostSpeed += boostAcceleration;
                    isBoosting = true;
                }
                else if (boostSpeed > 0)
                { 
                    boostSpeed -= boostAcceleration;
                    isBoosting = false;
                }
                boostSpeed = Mathf.Clamp(boostSpeed, 0, boostMax);

                // move
                if (!isTankControls)
                {
                    Vector3 move = transform.right * horizontal + transform.forward * vertical;

                    controller.Move(move * (speed+boostSpeed) * Time.deltaTime);

                    // boost up if not moving
                    if (isBoostVertical)
                    {
                        move = Vector3.up * boostSpeed * Time.deltaTime;
                        controller.Move(move);
                    }
                }
                else 
                {
                    moveDirection += horizontal * turnSpeed * Time.deltaTime;
                    moveRotation = Quaternion.AngleAxis(moveDirection, Vector3.up);
                    Vector3 movementVector = moveRotation * Vector3.forward * vertical * speed * Time.deltaTime;
                    controller.Move(movementVector);

                    Debug.Log(moveDirection * vertical);
                }

                // jump
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHiehgt * -2f * gravity);
                }

                // change and apply gravity
                if (!isGrounded && !isBoosting)
                {
                    velocity.y += gravity * Time.deltaTime;
                }
                else
                {
                    velocity.y = 0;
                }

                controller.Move(velocity * Time.deltaTime);

                break;
            case State.Dead:
                break;
        }
    }

    void Slide()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            if (angle > 0 && angle <= maxSlideAngle)
            {
                //rb.AddForce(Vector3.down * slideForce, ForceMode.Force);
                //rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }
    }
}
