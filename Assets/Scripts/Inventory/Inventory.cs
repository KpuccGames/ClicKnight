using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class Inventory : MonoBehaviour
{
    public InventoryCell[] m_InventoryCells;

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
    public void Show()
    {
        gameObject.SetActive(true);

        InitView();
    }

    ////////////////
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    ////////////////
    public void InitView()
    {
        List<EquipmentItem> items = InventoryContent.Instance.PlayerEquipments;

        for (int i = 0; i < items.Count; i++)
        {
            m_InventoryCells[i].SetItemIcon(items[i].GetIcon());
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
