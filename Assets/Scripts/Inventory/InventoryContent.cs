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
    public void AddMaterial(string materialName, int amount = 1)
    {
        MaterialData material = GameDataStorage.Instance.GetMaterialByName(materialName);

        if (material != null)
            AddMaterial(material, amount);
    }

    /////////////////
    public void AddMaterial(MaterialData material, int amount = 1)
    {
        if (material == null)
            return;

        MaterialInfo materialToAdd = null;

        foreach (MaterialInfo materialInfo in PlayerMaterials)
        {
            // если в инвентаре есть такой материал, то добавляем
            if (materialInfo.Data.Name == material.Name)
            {
                materialInfo.AddMaterial(amount);
                materialToAdd = materialInfo;
                break;
            }
        }

        // если материала в инвентаре не было, то создаем и прибавляем
        if (materialToAdd == null)
        {
            materialToAdd = new MaterialInfo(material, amount);
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
    private MaterialInfo GetMaterialInfo(MaterialData data)
    {
        if (data == null)
            return null;

        MaterialInfo info = PlayerMaterials.Find((mat) => mat.Data.Name.Equals(data.Name));

        return info;
    }

    /////////////////
    public bool TryRemoveMaterial(MaterialData data, int amount)
    {
        MaterialInfo info = GetMaterialInfo(data);

        if (info == null)
            return false;

        if (info.TryRemoveMaterial(amount))
        {
            if (info.Amount <= 0)
                PlayerMaterials.Remove(info);

            return true;
        }

        return false;
    }

    /////////////////
    public int GetMaterialAmount(MaterialData data)
    {
        MaterialInfo info = GetMaterialInfo(data);

        if (info == null)
            return 0;

        return info.Amount;
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
