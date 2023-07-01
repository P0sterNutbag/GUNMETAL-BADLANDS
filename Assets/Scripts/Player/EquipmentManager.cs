using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public Head myHead;
    public GunStats myGunLeft;
    public GunStats myGunRight;
    public Legs myLegs;
    public GameObject HeadObject;
    public PlayerGun GunLeftObject;
    public PlayerGun GunRightObject;
    public PlayerMovement LegsObject;

    public void Equip(Equipment newItem, EquipmentSlot slot)
    {
        switch (slot)
        {
            case EquipmentSlot.Head:
                if (newItem.GetType() == typeof(Head))
                {
                    myHead = (Head)newItem;
                    //HeadObject.stats = myHead;
                }
                break;

            case EquipmentSlot.GunLeft:
                if (newItem.GetType() == typeof(GunStats))
                {
                    myGunLeft = (GunStats)newItem;
                    GunLeftObject.stats = myGunLeft;
                }
                break;

            case EquipmentSlot.GunRight:
                if (newItem.GetType() == typeof(GunStats))
                {
                    myGunRight = (GunStats)newItem;
                    GunRightObject.stats = myGunRight;
                }
                break;

            case EquipmentSlot.Legs:
                if (newItem.GetType() == typeof(Legs))
                {
                    myLegs = (Legs)newItem;
                    LegsObject.stats = myLegs;
                }
                break;
        }
    }
}
