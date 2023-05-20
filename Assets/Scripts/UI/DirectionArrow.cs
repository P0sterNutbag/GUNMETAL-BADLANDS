using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    public RectTransform myTransform;
    public PlayerMovement player;
    public GameObject fpsCam;


    void Update()
    {
        float camDir = fpsCam.transform.eulerAngles.y;
        float playerDir = player.moveDirection; //* (Mathf.Abs(player.currentSpeed));
        myTransform.rotation = Quaternion.AngleAxis(playerDir - camDir, Vector3.up);
    }
}
