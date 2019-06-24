using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryCell : MonoBehaviour, IPointerClickHandler
{
    public Image m_Icon;

    private EquipmentItem m_EquipmentItem;
    private MaterialData m_MaterialItem;

    //////////////
    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;

        if (clickCount != 2 || m_EquipmentItem == null)
            return;

        PlayerProfile.Instance.EquipItem(m_EquipmentItem);
    }

    //////////////
    public void SetItem(IItem item)
    {
        if (item == null)
        {
            SetItemIcon(null);
            return;
        }

        ItemType type = item.GetItemType();

        if (type == ItemType.equipment)
        {
            m_EquipmentItem = (EquipmentItem)item;

            SetItemIcon(m_EquipmentItem.GetIcon());
        }
        else if (type == ItemType.material)
        {
            m_MaterialItem = (MaterialData)item;

            SetItemIcon(m_MaterialItem.GetIcon());
        }
    }

    //////////////
    private void SetItemIcon(Sprite icon)
    {
        m_Icon.overrideSprite = icon;
    }
}
