using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : ScriptableObject 
{
    public EquipmentSlot equipSlot;

    public void Equip()
    {
        EquipmentManager.instance.Equip(this);
    }
}

public enum EquipmentSlot { Torso, GunLeft, GunRight, Legs }