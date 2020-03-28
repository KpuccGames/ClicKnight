using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public enum EquipmentSlot
{
    unknown = -1,
    helmet = 0,
    chest = 1,
    legs = 2,
    boots = 3,
    shoulders = 4,
    gloves = 5,
    shield = 6,
    weapon = 7,
    count = 8
}

public class EquipmentItem : IItem
{
    public string Name { get; private set; }
    public int AttackBonus { get; private set; }
    public int HealthBonus { get; private set; }
    public EquipmentSlot Slot { get; private set; }

    private string m_IconPath;

    /////////////////
    public EquipmentItem(JsonObject json)
    {
        Name = (string)json["name"];
        AttackBonus = json.GetInt("damage");
        HealthBonus = json.GetInt("health");
        m_IconPath = (string)json["icon"];
        Slot = Helper.ParseEnum((string)json["slot"], EquipmentSlot.unknown);
    }

    /////////////////
    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>(m_IconPath);
    }

    /////////////////
    public ItemType GetItemType()
    {
        return ItemType.Equipment;
    }
}
