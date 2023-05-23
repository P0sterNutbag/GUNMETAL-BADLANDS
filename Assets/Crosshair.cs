using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public PlayerGun gun;
    public RectTransform barTop;
    public RectTransform barRight;
    public RectTransform barBottom;
    public RectTransform barLeft;

    float barTopOrg;
    float barRightOrg;
    float barBottomOrg;
    float barLeftOrg;

    private void Start()
    {
        barTopOrg = barTop.localPosition.y;
        barRightOrg = barRight.localPosition.x;
        barBottomOrg = barBottom.localPosition.y;
        barLeftOrg = barLeft.localPosition.x;
    }

    void Update()
    {
        float posChange = gun.aimVariance*100f;
        //Debug.Log(posChange);
        barTop.localPosition = new Vector3(0f, barTopOrg + posChange, 0f);
        barRight.localPosition = new Vector3(barRightOrg + posChange, 0f, 0f);
        barBottom.localPosition = new Vector3(0f, barBottomOrg - posChange, 0f);
        barLeft.localPosition = new Vector3(barLeftOrg - posChange, 0f, 0f);
    }
}
