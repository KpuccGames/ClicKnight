using System.Text;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryDialog : BaseDialog
{
    public InventoryCell[] m_InventoryCells;
    public HeroEquipmentView m_EquipmentView;
    public TextMeshProUGUI m_ItemStatsText;

    ////////////////
    private void OnEnable()
    {
        InventoryContent.OnInventoryContentChanged += UpdateView;
        InventoryCell.OnItemSelected += UpdateSelectedItemInfo;
    }

    ////////////////
    private void OnDisable()
    {
        InventoryContent.OnInventoryContentChanged -= UpdateView;
        InventoryCell.OnItemSelected -= UpdateSelectedItemInfo;
    }

    ////////////////
    public override void Show()
    {
        if (gameObject.activeInHierarchy)
        {
            GameManager.Instance.SaveGame();
            Hide();
            return;
        }

        base.Show();

        ShowEquipments();

        m_EquipmentView.UpdateEquipmentView();
    }

    ////////////////
    private void UpdateView(IItem item)
    {
        if (item.GetItemType() == ItemType.equipment)
        {
            ShowEquipments();
        }
        else
        {
            ShowMaterials();
        }
    }

    ////////////////
    public void ShowEquipments()
    {
        List<EquipmentItem> items = InventoryContent.Instance.PlayerEquipments;

        for (int i = 0; i < m_InventoryCells.Length; i++)
        {
            if (i >= items.Count)
            {
                m_InventoryCells[i].SetItem(null);

                continue;
            }

            m_InventoryCells[i].SetItem(items[i]);
        }
    }

    ////////////////
    public void ShowMaterials()
    {
        List<MaterialInfo> materials = InventoryContent.Instance.PlayerMaterials;

        for (int i = 0; i < (m_InventoryCells.Length); i++)
        {
            if (i >= (materials.Count))
            {
                m_InventoryCells[i].SetItem(null);

                continue;
            }

            m_InventoryCells[i].SetItem(materials[i]);
        }
    }

    ////////////////
    private void UpdateSelectedItemInfo(IItem item)
    {
        if (item == null)
        {
            m_ItemStatsText.text = string.Empty;
            return;
        }

        // TODO добавить информацию о материалах
        StringBuilder sb = new StringBuilder();

        if (item.GetItemType() == ItemType.equipment)
        {
            EquipmentItem equipment = (EquipmentItem)item;

            sb.AppendLine(equipment.Name);
            sb.AppendLine("Attack: " + equipment.AttackBonus);
            sb.AppendLine("Health: " + equipment.HealthBonus);
        }
        else
        {
            MaterialInfo material = (MaterialInfo)item;

            sb.AppendLine(material.Data.Name);
        }

        m_ItemStatsText.text = sb.ToString();
    }

    /////////////////
    public void CheatAddItem()
    {
        int index = Random.Range(0, GameDataStorage.Instance.Equipments.Count);

        EquipmentItem item = GameDataStorage.Instance.Equipments[index];

        InventoryContent.Instance.AddEquipmentItem(item.Name);
    }
}
