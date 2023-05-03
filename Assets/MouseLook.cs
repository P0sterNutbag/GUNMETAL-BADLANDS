using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 200f;
    public bool isHolding = false;
    private float rotationY = 0f;

    public Transform playerBody;
    public PlayerMovement player;
    public LayerMask groundMask;

    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        switch (player.currentState)
        {
            case PlayerMovement.State.Walk:
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                yRotation = mouseX;

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);
                break;
            case PlayerMovement.State.Dead:
                bool isGrounded = Physics.CheckSphere(transform.position, 0.4f, groundMask);
                if (!isGrounded)
                {
                    transform.Translate(Vector3.down * 10f * Time.deltaTime);
                }
                break;
        }
    }
}
