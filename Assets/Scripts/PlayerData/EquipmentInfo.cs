using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInfo : IItem
{
    public EquipmentData Data { get; }

    public EquipmentInfo(EquipmentData data)
    {
        Data = data;
    }

    /////////////////
    public ItemType GetItemType()
    {
        return ItemType.Equipment;
    }
}
