using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsPosition : MonoBehaviour
{
    public Transform fpsCam;
    public float rotationSpeed = 5f;
    private Vector3 playerdif;
    private Vector3 camdif;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerdif = new Vector3(transform.position.x - player.position.x, transform.position.y - player.position.y, transform.position.z - player.position.z);
        camdif = new Vector3(transform.position.x - fpsCam.position.x, transform.position.y - fpsCam.position.y, transform.position.z - fpsCam.position.z);
    }

    private void LateUpdate()
    {
        transform.position = fpsCam.position + camdif;
        Quaternion targetRotation = Quaternion.Euler(fpsCam.eulerAngles.x, player.rotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
