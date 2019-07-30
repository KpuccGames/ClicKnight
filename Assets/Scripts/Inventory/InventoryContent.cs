using SimpleJson;
using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryContent
{
    private static InventoryContent m_Instance;
    public static InventoryContent Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new InventoryContent();

            return m_Instance;
        }
    }

    public List<EquipmentItem> PlayerEquipments { get; private set; }
    public List<MaterialInfo> PlayerMaterials { get; private set; }

    public bool IsInited { get; private set; }

    public static event Action<IItem> OnInventoryContentChanged;

    /////////////////
    public InventoryContent()
    {
        PlayerEquipments = new List<EquipmentItem>();
        PlayerMaterials = new List<MaterialInfo>();
    }

    /////////////////
    public void Init(JsonObject json)
    {
        if (json == null)
            return;

        JsonArray equipments = json.Get<JsonArray>("equipments");

        for (int i = 0; i < equipments.Count; i++)
        {
            string obj = (string)equipments[i];
            EquipmentItem item = GameDataStorage.Instance.GetEquipmentByName(obj);

            PlayerEquipments.Add(item);
        }

        JsonArray materials = json.Get<JsonArray>("materials");

        foreach (JsonObject item in materials)
        {
            MaterialData data = GameDataStorage.Instance.GetMaterialByName((string)item["name"]);
            int amount = item.GetInt("amount");

            PlayerMaterials.Add(new MaterialInfo(data, amount));
        }
    }

    /////////////////
    public void AddMaterial(MaterialData material)
    {
        if (material == null)
            return;

        MaterialInfo materialToAdd = null;

        foreach (MaterialInfo materialInfo in PlayerMaterials)
        {
            if (materialInfo.Data.Name == material.Name)
            {
                materialInfo.AddMaterial(1);
                materialToAdd = materialInfo;
                break;
            }
        }

        if (materialToAdd == null)
        {
            materialToAdd = new MaterialInfo(material, 1);
            PlayerMaterials.Add(materialToAdd);
        }

        if (OnInventoryContentChanged != null)
            OnInventoryContentChanged(materialToAdd);
    }

    /////////////////
    public void AddEquipmentItem(string itemName)
    {
        EquipmentItem item = GameDataStorage.Instance.GetEquipmentByName(itemName);

        if (item != null)
            AddEquipmentItem(item);
    }

    /////////////////
    public void AddEquipmentItem(EquipmentItem item)
    {
        if (item != null)
        {
            PlayerEquipments.Add(item);

            //
            // TODO: подумать, надо ли сохранять содержимое, если игрок не прошел уровень
            // интереснее в конце уровня показать полученный дроп
            //

            if (OnInventoryContentChanged != null)
                OnInventoryContentChanged(item);
        }
    }

    /////////////////
    public void RemoveItem(EquipmentItem itemToRemove)
    {
        foreach (EquipmentItem item in PlayerEquipments)
        {
            if (item == itemToRemove)
            {
                PlayerEquipments.Remove(item);

                if (OnInventoryContentChanged != null)
                    OnInventoryContentChanged(itemToRemove);

                return;
            }
        }
    }
}
