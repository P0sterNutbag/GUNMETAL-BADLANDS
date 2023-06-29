using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public Legs stats;
    #region old stats
    [Header("Movement")]   
    /*float states.speed = 12f;
    public float states.accelerationSpd = 0.1f;
    public float states.turnSpeed = 180f; // for tank controls
    public float states.gravity = -9.81f;
    public bool states.isTankControls;*/
    #endregion
    [Header("Boost")]
    public float boostSpeedMax = 20f;
    public float boostAcceleration = 0.02f;
    public float boostMax = 12f;
    public float boostTime = 60f;
    public float boostTimeMax = 60f;
    public float boostTimeChargeSpd = 0.025f;
    public float boostCooldownTime = 1f;
    [Header("Gameobjects")]
    public Transform groundCheck;
    public LayerMask groundMask;
    public GameObject boostbar;

    [HideInInspector]
    public float moveDirection = 0f;
    public float currentSpeed = 0f;
    public float boostSpeed = 0f;
    private float groundDistance = 0.4f;

    private bool isBoosting = false;
    private bool isBoostVertical = false;
    private Quaternion moveRotation;
    private Vector3 move;

    public enum State
    { 
        Walk,
        Dead
    }
    public State currentState = State.Walk;


    Vector3 velocity;
    public bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        // state machine
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
                    if ((horizontal == 0 && vertical == 0) || states.isTankControls) //|| (!isGrounded))
                    {
                        isBoostVertical = true;
                    }
                    else
                    {
                        isBoostVertical = false;
                    }
                }
                // boost
                if (Input.GetKey("left shift") && boostTime > 0)
                {
                    if (boostSpeed < boostSpeedMax)
                        {
                        boostSpeed += boostAcceleration;
                    }
                    isBoosting = true;
                    boostTime -= Time.deltaTime;
                    if (boostTime <= 0)
                    {
                        boostTime = -boostCooldownTime;
                    }
                }
                else
                {
                    isBoosting = false;
                    // recharge
                    boostTime += boostTimeChargeSpd;
                    if (boostTime >= boostTimeMax)
                    {
                        boostTime = boostTimeMax;
                    }
                    // slow boost speed
                    if (boostSpeed > 0)
                    {
                        boostSpeed -= boostAcceleration;
                    }
                }
                boostSpeed = Mathf.Clamp(boostSpeed, 0, boostMax);
                boostbar.GetComponent<Healthbar>().SetHealth(boostTime, boostTimeMax);

                // move
                if (states.isTankControls)
                {
                    // determine speed
                    if (vertical != 0)
                    {
                        currentSpeed += states.accelerationSpd * vertical * Time.deltaTime;
                    }
                    currentSpeed = Mathf.Clamp(currentSpeed, -speed, speed);
                    // determine direction
                    moveDirection += horizontal * states.turnSpeed * Time.deltaTime;
                    moveRotation = Quaternion.AngleAxis(moveDirection, Vector3.up);
                    Vector3 movementVector = moveRotation * Vector3.forward * currentSpeed * Time.deltaTime;
                    if (isGrounded && !isBoosting) { movementVector.y = -4.5f; } // get rid of bumps down slopes
                    controller.Move(movementVector);
                }
                else 
                {
                    Vector3 move = transform.right * horizontal + transform.forward * vertical;
                    controller.Move(move * (speed + boostSpeed) * Time.deltaTime);
                }

                // boost up if not moving
                if (isBoostVertical)
                {
                    Vector3 newMove = Vector3.up * boostSpeed * Time.deltaTime;
                    controller.Move(newMove);
                }

                // change and apply gravity
                if (!isGrounded && !isBoosting)
                {
                    velocity.y += states.gravity * Time.deltaTime;
                }
                else
                {
                    velocity.y = 0;
                }

                // brake
                if (states.isTankControls && Input.GetButton("Brake"))
                {
                    if (currentSpeed > 0)
                    {
                        currentSpeed -= states.accelerationSpd * Time.deltaTime;
                        if (currentSpeed < 0)  currentSpeed = 0;
                    }
                    else if (currentSpeed < 0)
                    {
                        currentSpeed += states.accelerationSpd * Time.deltaTime;
                        if (currentSpeed > 0)  currentSpeed = 0;
                    }
                }

                controller.Move(velocity * Time.deltaTime);

                break;
            case State.Dead:
                break;
        }
    }
}
