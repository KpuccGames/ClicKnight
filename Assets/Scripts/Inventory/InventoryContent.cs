﻿using SimpleJson;
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

        OnInventoryContentChanged += SaveInventoryData;
    }

    /////////////////
    public void Init(JsonObject json)
    {
        if (json == null)
            return;

        JsonArray equipments = json.Get<JsonArray>("equipments");

        foreach (JsonObject obj in equipments)
        {
            EquipmentItem item = GameDataStorage.Instance.GetEquipmentByName((string)json["name"]);

            PlayerEquipments.Add(item);
        }
    }

    /////////////////
    public void AddItem(string itemName)
    {
        EquipmentItem item = GameDataStorage.Instance.GetEquipmentByName(itemName);

        if (item != null)
        {
            PlayerEquipments.Add(item);

            if (OnInventoryContentChanged != null)
                OnInventoryContentChanged();
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
        PlayerPrefs.SetString("saved_data", dataToSave.ToString());
        PlayerPrefs.Save();
    }
}