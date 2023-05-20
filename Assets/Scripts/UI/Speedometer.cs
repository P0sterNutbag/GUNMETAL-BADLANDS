using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public RectTransform arrow;
    public PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dir = -player.currentSpeed * 20f;
        arrow.rotation = Quaternion.AngleAxis(dir, Vector3.forward);
    }
}
