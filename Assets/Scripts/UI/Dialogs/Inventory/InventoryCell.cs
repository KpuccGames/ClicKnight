using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    public Image m_Icon;
    public TextMeshProUGUI m_AmountText;
    public Button m_CellButton;

    public IItem Item { get; private set; }

    public static event Action<InventoryCell> OnItemCellClicked;

    private void OnEnable()
    {
        OnItemCellClicked += UpdateCell;
    }

    private void OnDisable()
    {
        OnItemCellClicked -= UpdateCell;
    }

    //////////////
    public void OnClick()
    {
        OnItemCellClicked?.Invoke(this);
    }
    
    //////////////
    public void OnClickCellButton()
    {
        if (Item == null)
            return;

        if (Item.GetItemType() == ItemType.Material)
        {
            MaterialInfo materialInfo = (MaterialInfo)Item;

            PlayerProfile.Instance.AddItemToPocket(materialInfo.Data);
        }
        else
        {
            EquipmentData equip = ((EquipmentInfo)Item).Data;

            PlayerProfile.Instance.EquipItem(equip);
        }

        m_CellButton.gameObject.SetActive(false);
    }

    //////////////
    public void SetItem(IItem item)
    {
        Item = item;

        if (item == null)
        {
            SetItemIcon(null);
            return;
        }
        
        ItemType type = item.GetItemType();
        
        if (type == ItemType.Equipment)
        {
            m_AmountText.gameObject.SetActive(false);

            SetItemIcon(((EquipmentInfo)item).Data.GetIcon());
        }
        else if (type == ItemType.Material)
        {
            MaterialInfo material = (MaterialInfo)item;

            SetItemIcon(material.Data.GetIcon());

            m_AmountText.gameObject.SetActive(true);
            m_AmountText.text = material.Amount.ToString();
        }
    }

    //////////////
    private void SetItemIcon(Sprite icon)
    {
        m_Icon.overrideSprite = icon;
    }

    private void UpdateCell(InventoryCell cell)
    {
        bool needShowButton = cell == this && cell.Item != null;

        m_CellButton.gameObject.SetActive(needShowButton);
    }
}
