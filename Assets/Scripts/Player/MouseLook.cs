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
    public float bobFrequency = 2f;       
    public float bobAmplitude = 0.05f;
    public float bobRiseSpeed = 2f;     
    public float bobFallSpeed = 5f;

    public Camera cam;
    public Transform playerBody;
    public GameObject gun;
    public PlayerMovement player;
    public LayerMask groundMask;

    private float timer = 0f;
    private Vector3 originalPosition;
    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        defaultFov = cam.fieldOfView;
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        switch (player.currentState)
        {
            case PlayerMovement.State.Walk:
                // look towards mouse
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                yRotation += mouseX;
                yRotation = Mathf.Clamp(yRotation, player.moveDirection-90f, player.moveDirection+90f);

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.localRotation = Quaternion.Euler(0f, yRotation, 0f);

                // headbob
                if (player.currentSpeed != 0)
                {
                    float speedRatio = player.currentSpeed / player.speed;
                    float riseSpeed = bobRiseSpeed * speedRatio;
                    float fallSpeed = bobFallSpeed * speedRatio;
                    float frequency = bobFrequency * speedRatio;
                    float verticalOffset = Mathf.Sin(timer) * bobAmplitude;
                    Vector3 newPosition = originalPosition + new Vector3(0f, verticalOffset, 0f);
                    transform.localPosition = newPosition;
                    timer += frequency * Time.deltaTime;
                    float bobSpeed = verticalOffset > 0f ? riseSpeed : fallSpeed;
                    timer += bobSpeed * Time.deltaTime;
                }

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
