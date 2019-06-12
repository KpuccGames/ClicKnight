using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeroEquipmentCell : MonoBehaviour, IPointerClickHandler
{
    public EquipmentSlot m_EquipmentSlot;
    public Image m_EquipmentIcon;

    private EquipmentItem m_EquipmentItem;

    ////////////////
    public void SetItem(EquipmentItem item)
    {
        if (item == null)
        {
            m_EquipmentIcon.overrideSprite = null;
            return;
        }

        if (item.Slot != m_EquipmentSlot)
        {
            return;
        }

        m_EquipmentItem = item;
        m_EquipmentIcon.overrideSprite = item.GetIcon();
    }

    //////////////
    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;

        if (clickCount != 2 || m_EquipmentItem == null)
            return;

        PlayerProfile.Instance.UnequipItem(m_EquipmentItem);
    }
}
