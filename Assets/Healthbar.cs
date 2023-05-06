using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{

    public Slider slider;
    public void SetHealth(float health, float maxHealth)
    {
        float barVal = health / maxHealth;
        slider.value = barVal;
    }

}
