using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : ScriptableObject 
{
    public EquipmentSlot equipSlot;
}

public enum EquipmentSlot { Head, GunLeft, GunRight, Legs }