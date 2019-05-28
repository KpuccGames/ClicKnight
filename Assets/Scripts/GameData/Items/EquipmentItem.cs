using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public enum EquipmentSlot
{
    helmet = 0,
    chest = 1,
    legs = 2,
    boots = 3,
    shoulders = 4,
    gloves = 5,
    neck = 6,
    finger = 7
}

public class EquipmentItem : IItem
{
    public string Name { get; private set; }
    public int AttackBonus { get; private set; }
    public int ArmorBonus { get; private set; }

    private string m_IconPath;

    /////////////////
    public EquipmentItem(JsonObject json)
    {
        Name = (string)json["name"];
        AttackBonus = json.GetInt("damage");
        ArmorBonus = json.GetInt("armor");
        m_IconPath = (string)json["icon"];
    }

    /////////////////
    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>(m_IconPath);
    }

    /////////////////
    public ItemType GetItemType()
    {
        return ItemType.equipment;
    }
}
