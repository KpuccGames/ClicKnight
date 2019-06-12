using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryCell : MonoBehaviour, IPointerClickHandler
{
    public Image m_Icon;

    private EquipmentItem m_EquipmentItem;

    //////////////
    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;

        if (clickCount != 2 || m_EquipmentItem == null)
            return;

        PlayerProfile.Instance.EquipItem(m_EquipmentItem);
    }

    //////////////
    public void SetItem(EquipmentItem item)
    {
        if (item == null)
        {
            SetItemIcon(null);
            return;
        }

        m_EquipmentItem = item;

        SetItemIcon(item.GetIcon());
    }

    //////////////
    private void SetItemIcon(Sprite icon)
    {
        m_Icon.overrideSprite = icon;
    }
}
