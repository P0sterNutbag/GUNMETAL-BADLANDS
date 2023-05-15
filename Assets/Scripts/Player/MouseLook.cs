using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 200f;
    public bool isHolding = false;
    public float defaultFov;
    public bool isZooming = false;
    public float camZoomAmount = 2f;
    public float camZoomSpd = 0.25f;

    public Camera cam;
    public Transform playerBody;
    public GameObject gun;
    public PlayerMovement player;
    public LayerMask groundMask;

    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        defaultFov = cam.fieldOfView;
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

        // zoom in camera
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            isZooming = true;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            isZooming = false;
        }
        if (isZooming)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFov/ camZoomAmount, camZoomSpd);
        }
        else 
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFov, camZoomSpd);
        }
    }
}
