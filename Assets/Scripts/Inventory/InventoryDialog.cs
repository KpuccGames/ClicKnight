using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class InventoryDialog : BaseDialog
{
    public InventoryCell[] m_InventoryCells;
    public HeroEquipmentView m_EquipmentView;

    ////////////////
    private void OnEnable()
    {
        InventoryContent.OnInventoryContentChanged += InitView;
    }

    ////////////////
    private void OnDisable()
    {
        InventoryContent.OnInventoryContentChanged -= InitView;
    }

    ////////////////
    public override void Show()
    {
        base.Show();

        InitView();

        m_EquipmentView.UpdateEquipmentView();
    }

    ////////////////
    public void InitView()
    {
        List<EquipmentItem> items = InventoryContent.Instance.PlayerEquipments;

        int i = 0;

        for (; i < m_InventoryCells.Length; i++)
        {
            if (i >= items.Count)
            {
                break;
            }

            m_InventoryCells[i].SetItem(items[i]);
        }

        List<MaterialData> materials = InventoryContent.Instance.PlayerMaterials;

        for (int j = 0; j < (m_InventoryCells.Length - items.Count); j++)
        {
            if (j >= (materials.Count))
            {
                m_InventoryCells[i].SetItem(null);
                i++;

                continue;
            }

            m_InventoryCells[i].SetItem(materials[j]);
            i++;
        }
    }

    /////////////////
    public void CheatAddItem()
    {
        int index = Random.Range(0, GameDataStorage.Instance.Equipments.Count);

        EquipmentItem item = GameDataStorage.Instance.Equipments[index];

        InventoryContent.Instance.AddEquipmentItem(item.Name);
    }
}
