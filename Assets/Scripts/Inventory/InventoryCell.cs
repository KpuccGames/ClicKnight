using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryCell : MonoBehaviour, IPointerClickHandler
{
    public Image m_Icon;
    public TextMeshProUGUI m_AmountText;
    public Button m_EquipButton;

    private IItem m_Item;

    public static event Action<IItem> OnItemSelected;

    //////////////
    public void OnPointerClick(PointerEventData eventData)
    {
        OnItemSelected?.Invoke(m_Item);

        if (m_Item == null)
            return;

        m_EquipButton.gameObject.SetActive(!m_EquipButton.gameObject.activeSelf);
    }
    
    //////////////
    public void OnClickEquip()
    {
        if (m_Item == null || m_Item.GetItemType() == ItemType.material)
            return;

        EquipmentItem equip = (EquipmentItem)m_Item;

        PlayerProfile.Instance.EquipItem(equip);

        m_EquipButton.gameObject.SetActive(false);
    }

    //////////////
    public void SetItem(IItem item)
    {
        m_Item = item;

        if (item == null)
        {
            SetItemIcon(null);
            return;
        }
        
        ItemType type = item.GetItemType();

        SetItemIcon(item.GetIcon());

        if (type == ItemType.equipment)
        {
            m_AmountText.gameObject.SetActive(false);
        }
        else if (type == ItemType.material)
        {
            MaterialInfo material = (MaterialInfo)item;

            m_AmountText.gameObject.SetActive(true);
            m_AmountText.text = material.Amount.ToString();
        }
    }

    //////////////
    private void SetItemIcon(Sprite icon)
    {
        m_Icon.overrideSprite = icon;
    }
}
