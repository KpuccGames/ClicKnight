using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryCell : MonoBehaviour, IPointerClickHandler
{
    public Image m_Icon;
    public TextMeshProUGUI m_AmountText;
    public Button m_EquipButton;

    private EquipmentItem m_EquipmentItem;
    private MaterialInfo m_MaterialItem;

    //////////////
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_MaterialItem != null || m_EquipmentItem == null)
            return;

        m_EquipButton.gameObject.SetActive(!m_EquipButton.gameObject.activeSelf);
    }
    
    //////////////
    public void OnClickEquip()
    {
        if (m_EquipmentItem == null)
            return;
        
        PlayerProfile.Instance.EquipItem(m_EquipmentItem);
        m_EquipmentItem = null;

        m_EquipButton.gameObject.SetActive(false);
    }

    //////////////
    public void SetItem(IItem item)
    {
        m_AmountText.gameObject.SetActive(false);

        if (item == null)
        {
            SetItemIcon(null);
            return;
        }
        
        ItemType type = item.GetItemType();

        if (type == ItemType.equipment)
        {
            m_EquipmentItem = (EquipmentItem)item;
            m_MaterialItem = null;

            SetItemIcon(m_EquipmentItem.GetIcon());
        }
        else if (type == ItemType.material)
        {
            m_MaterialItem = (MaterialInfo)item;
            m_EquipmentItem = null;

            SetItemIcon(m_MaterialItem.GetIcon());

            m_AmountText.gameObject.SetActive(true);
            m_AmountText.text = m_MaterialItem.Amount.ToString();
        }
    }

    //////////////
    private void SetItemIcon(Sprite icon)
    {
        m_Icon.overrideSprite = icon;
    }
}
