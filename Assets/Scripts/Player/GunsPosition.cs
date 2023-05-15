using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsPosition : MonoBehaviour
{
    public Camera fpsCam;
    public float rotationSpeed = 5f;
    private Vector3 playerdif;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerdif = new Vector3(transform.position.x - player.position.x, transform.position.y - player.position.y, transform.position.z - player.position.z);
    }

    private void LateUpdate()
    {
        transform.position = player.position + playerdif;
        Quaternion targetRotation = Quaternion.Euler(fpsCam.transform.eulerAngles.x, player.rotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
