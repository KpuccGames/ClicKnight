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
        Inventory.OnInventoryContentChanged += UpdateView;
        InventoryCell.OnItemCellClicked += UpdateSelectedItemInfo;
    }

    ////////////////
    private void OnDisable()
    {
        Inventory.OnInventoryContentChanged -= UpdateView;
        InventoryCell.OnItemCellClicked -= UpdateSelectedItemInfo;
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
        if (item.GetItemType() == ItemType.Equipment)
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
        List<EquipmentInfo> items = Inventory.Instance.PlayerEquipments;

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
        List<MaterialInfo> materials = Inventory.Instance.PlayerMaterials;

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
        m_ItemStatsText.text = UIHelper.GetItemInformationText(item);
    }

    /////////////////
    public void CheatAddItem()
    {
        List<EquipmentData> datas = EquipmentsDataStorage.Instance.GetData();
        int index = Random.Range(0, datas.Count);

        EquipmentData item = datas[index];

        Inventory.Instance.AddEquipmentItem(item.Name);
    }
}
