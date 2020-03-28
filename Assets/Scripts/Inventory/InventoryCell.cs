using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryCell : MonoBehaviour, IPointerClickHandler
{
    public Image m_Icon;
    public TextMeshProUGUI m_AmountText;
    public Button m_CellButton;

    private IItem m_Item;

    public static event Action<IItem> OnItemCellClicked;

    //////////////
    public void OnPointerClick(PointerEventData eventData)
    {
        OnItemCellClicked?.Invoke(m_Item);

        if (m_Item == null)
            return;

        m_CellButton.gameObject.SetActive(!m_CellButton.gameObject.activeSelf);
    }
    
    //////////////
    public void OnClickCellButton()
    {
        if (m_Item == null)
            return;

        if (m_Item.GetItemType() == ItemType.Material)
        {
            MaterialInfo materialInfo = (MaterialInfo)m_Item;

            PlayerProfile.Instance.AddItemToPocket(materialInfo.Data);
        }
        else
        {
            EquipmentItem equip = (EquipmentItem)m_Item;

            PlayerProfile.Instance.EquipItem(equip);
        }

        m_CellButton.gameObject.SetActive(false);
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

        if (type == ItemType.Equipment)
        {
            m_AmountText.gameObject.SetActive(false);
        }
        else if (type == ItemType.Material)
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
