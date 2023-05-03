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

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isHolding = false;

    private bool isSliding = false;

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

                // move
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");

                Vector3 move = transform.right * x + transform.forward * z;

                controller.Move(move * speed * Time.deltaTime);

                // jump
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHiehgt * -2f * gravity);
                }

                // jump
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHiehgt * -2f * gravity);
                }

                // change and apply gravity
                velocity.y += gravity * Time.deltaTime;

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
