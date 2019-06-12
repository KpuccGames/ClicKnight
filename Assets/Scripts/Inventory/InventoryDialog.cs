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

    /////////////////
    public void CheatAddItem()
    {
        int index = Random.Range(0, GameDataStorage.Instance.Equipments.Count);

        EquipmentItem item = GameDataStorage.Instance.Equipments[index];

        InventoryContent.Instance.AddItem(item.Name);
    }
}
