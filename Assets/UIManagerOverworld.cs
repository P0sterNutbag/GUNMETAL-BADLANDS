using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerOverworld : MonoBehaviour
{
    public float npcOptionsXoffset;

    public GameObject npcOptions;
    public Transform targetTransform;
    public RectTransform uiRectangle;

    void Update()
    {
        if (targetTransform == null)
        {
            if (npcOptions.activeSelf == true)
            {
                npcOptions.SetActive(false);
            }
        }
        else
        {
            if (npcOptions.activeSelf == false)
            {
                npcOptions.SetActive(true);
            }

            RectTransform textTransform = (RectTransform)npcOptions.GetComponent<RectTransform>().Find("Text");
            RectTransform outlineTransform = (RectTransform)npcOptions.GetComponent<RectTransform>().Find("Outline");

            // set options position
            Vector3 targetPos = Camera.main.WorldToScreenPoint(targetTransform.position);
            Vector3 textPos = new Vector3(targetPos.x + npcOptionsXoffset, targetPos.y, targetPos.z);
            textTransform.position = textPos;

            // set rectangle around target transform
            outlineTransform.position = targetPos;
        }
    }
}
