using System.Text;
using System.Collections.Generic;
using UnityEngine;

public static class UIHelper
{
    ////////////////
    public static string GetItemInformationText(IItem item)
    {
        if (item == null)
        {
            return string.Empty;
        }

        StringBuilder sb = new StringBuilder();

        if (item.GetItemType() == ItemType.Equipment)
        {
            EquipmentData equipment = ((EquipmentInfo)item).Data;

            sb.AppendLine(equipment.Name);
            sb.AppendLine("Attack: " + equipment.AttackBonus);
            sb.AppendLine("Health: " + equipment.HealthBonus);
        }
        else
        {
            MaterialInfo material = (MaterialInfo)item;

            sb.AppendLine(material.Data.Name);
        }

        return sb.ToString();
    }
}
