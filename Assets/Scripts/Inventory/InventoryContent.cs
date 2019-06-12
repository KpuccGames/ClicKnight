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

    public bool IsInited { get; private set; }

    public static event Action OnInventoryContentChanged;

    /////////////////
    public InventoryContent()
    {
        PlayerEquipments = new List<EquipmentItem>();
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
    }

    /////////////////
    public void AddItem(string itemName)
    {
        EquipmentItem item = GameDataStorage.Instance.GetEquipmentByName(itemName);

        if (item != null)
            AddItem(item);
    }

    /////////////////
    public void AddItem(EquipmentItem item)
    {
        if (item != null)
        {
            PlayerEquipments.Add(item);

            //
            // TODO: подумать, надо ли сохранять содержимое, если игрок не прошел уровень
            // интереснее в конце уровня показать полученный дроп
            //
            SaveInventoryData();

            if (OnInventoryContentChanged != null)
                OnInventoryContentChanged();
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
                Debug.Log("removed " + item.Name);
                SaveInventoryData();

                if (OnInventoryContentChanged != null)
                    OnInventoryContentChanged();

                return;
            }
        }
    }

    /////////////////
    private void SaveInventoryData()
    {
        //
        // TODO: сохранять данные инвентаря при изменении содержимого
        //

        JsonObject dataToSave = new JsonObject();

        if (PlayerEquipments.Count > 0)
        {
            JsonArray equipments = new JsonArray();

            foreach (EquipmentItem item in PlayerEquipments)
            {
                equipments.Add(item.Name);
            }

            dataToSave.Add("equipments", equipments);
        }

        // после формирования файла сохраняем
        PlayerPrefs.SetString(Constants.SavedGame, dataToSave.ToString());
        PlayerPrefs.Save();
    }
}
