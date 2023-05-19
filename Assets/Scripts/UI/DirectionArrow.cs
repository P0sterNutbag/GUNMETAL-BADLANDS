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
        myTransform.rotation = Quaternion.AngleAxis(player.moveDirection - camDir, Vector3.up);
        Debug.Log(camDir - player.moveDirection);
    }
}
