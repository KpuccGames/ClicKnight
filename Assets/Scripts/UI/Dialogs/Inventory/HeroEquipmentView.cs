using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroEquipmentView : MonoBehaviour
{
    public HeroEquipmentCell[] m_EquipmentCells;

    ///////////////
    private void OnEnable()
    {
        PlayerProfile.OnEquipmentChanged += UpdateEquipmentView;
    }

    ///////////////
    private void OnDisable()
    {
        PlayerProfile.OnEquipmentChanged -= UpdateEquipmentView;
    }

    ///////////////
    public void UpdateEquipmentView()
    {
        foreach (HeroEquipmentCell cell in m_EquipmentCells)
        {
            EquipmentData item;

            if (PlayerProfile.Instance.HeroEquipment.TryGetValue(cell.m_EquipmentSlot, out item))
                cell.SetItem(item);
            else
                cell.SetItem(null);
        }
    }

    ///////////////
    private HeroEquipmentCell GetEquipmentCell(EquipmentSlot slot)
    {
        foreach (HeroEquipmentCell cell in m_EquipmentCells)
        {
            if (cell.m_EquipmentSlot == slot)
                return cell;
        }

        return null;
    }
}
