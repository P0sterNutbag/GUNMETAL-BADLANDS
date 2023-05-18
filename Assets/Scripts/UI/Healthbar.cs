using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(float val, float maxVal)
    {
        float barVal = val / maxVal;
        slider.value = barVal;
    }

}
