using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class PlayerProfile
{
    private static PlayerProfile m_Instance;
    public static PlayerProfile Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new PlayerProfile();

            return m_Instance;
        }
    }

    public int Health { get; private set; }
    public int BaseDamage { get; private set; }
    public Dictionary<EquipmentSlot, EquipmentData> HeroEquipment { get; private set; } = new Dictionary<EquipmentSlot, EquipmentData>();
    public MaterialInfo[] PocketItems { get; private set; } = new MaterialInfo[4];

    public int NormalWorldMissionNumber { get; private set; }
    public int FireWorldMissionNumber { get; private set; }
    public int WaterWorldMissionNumber { get; private set; }
    public int AirWorldMissionNumber { get; private set; }
    public int EarthWorldMissionNumber { get; private set; }
    public int DarknessWorldMissionNumber { get; private set; }

    public static event Action OnEquipmentChanged;

    ///////////////
    public void CreateNewProfile(JsonArray data)
    {
        JsonObject json = data.GetAt<JsonObject>(0);

        InitBaseStats(data);
        NormalWorldMissionNumber = json.GetInt(Constants.NormalWorldMissionNumber);
        FireWorldMissionNumber = json.GetInt(Constants.FireWorldMissionNumber);
        WaterWorldMissionNumber = json.GetInt(Constants.WaterWorldMissionNumber);
        AirWorldMissionNumber = json.GetInt(Constants.AirWorldMissionNumber);
        EarthWorldMissionNumber = json.GetInt(Constants.EarthWorldMissionNumber);
        DarknessWorldMissionNumber = json.GetInt(Constants.DarknessWorldMissionNumber);

        HeroEquipment = new Dictionary<EquipmentSlot, EquipmentData>();
    }

    ///////////////
    public int GetDamage()
    {
        int damage = BaseDamage;

        foreach (EquipmentData equipment in HeroEquipment.Values)
        {
            damage += equipment.AttackBonus;
        }

        return damage;
    }

    ///////////////
    public int GetHealth()
    {
        int health = Health;

        foreach (EquipmentData equipment in HeroEquipment.Values)
        {
            health += equipment.HealthBonus;
        }

        return health;
    }

    ///////////////
    public void EquipItem(EquipmentData item)
    {
        if (item == null)
            return;

        EquipmentData equippedItem;

        // если на персонаже нет предмета в этом слоте, тогда надеваем предмет
        if (!HeroEquipment.TryGetValue(item.Slot, out equippedItem))
        {
            HeroEquipment.Add(item.Slot, item);
            Inventory.Instance.RemoveItem(item);

            if (OnEquipmentChanged != null)
                OnEquipmentChanged();

            return;
        }

        Inventory.Instance.AddEquipmentItem(equippedItem);
        Inventory.Instance.RemoveItem(item);
        HeroEquipment[item.Slot] = item;

        if (OnEquipmentChanged != null)
            OnEquipmentChanged();
    }

    ///////////////
    public void UnequipItem(EquipmentData item)
    {
        if (item == null)
            return;

        HeroEquipment.Remove(item.Slot);
        Inventory.Instance.AddEquipmentItem(item);

        if (OnEquipmentChanged != null)
            OnEquipmentChanged();
    }

    ///////////////
    private void TryUpdateProgress(MissionData completedMission)
    {
        switch (completedMission.World)
        {
            case MissionWorld.Normal:
                int number = completedMission.Number;

                // если первый раз прошли миссию
                if (number == NormalWorldMissionNumber)
                {
                    NormalWorldMissionNumber++;

                    // 
                    // NOTE: имитация туториала, потом привести в нормальный вид
                    //
                    switch (number)
                    {
                        case 2:
                            Inventory.Instance.AddEquipmentItem("axe");
                            break;

                        case 3:
                            Inventory.Instance.AddMaterial("ingot", 15);
                            break;
                    }
                }
                break;
        }

        GameManager.Instance.SaveGame();
    }

    ///////////////
    public void AddItemToPocket(MaterialData data)
    {
        if (IsPocketFull())
            return;

        if (!Inventory.Instance.TryRemoveMaterial(data, 1))
            return;

        for (int i = 0; i < PocketItems.Length; i++)
        {
            if (PocketItems[i] == null)
            {
                PocketItems[i] = new MaterialInfo(data, 1);
                break;
            }
        }
    }

    ///////////////
    public void RemoveItemFromPocket(int pocketNumber, bool isUsed)
    {
        if (pocketNumber < 0 || pocketNumber >= PocketItems.Length)
            return;

        if (PocketItems[pocketNumber] == null)
            return;

        if (!isUsed)
        {
            Inventory.Instance.AddMaterial(PocketItems[pocketNumber].Data);
        }

        PocketItems[pocketNumber] = null;
    }

    private bool IsPocketFull()
    {
        for (int i = 0; i < PocketItems.Length; i++)
        {
            if (PocketItems[i] == null)
                return false;
        }

        return true;
    }

    //Utils//

    ///////////////
    public void LoadProfile(JsonObject json, JsonArray baseConfig)
    {
        InitBaseStats(baseConfig);

        // экипировка героя
        JsonArray heroEquipments = json.Get<JsonArray>("hero_equipment");

        foreach (JsonObject obj in heroEquipments)
        {
            EquipmentData item = EquipmentsDataStorage.Instance.GetByName(obj.GetString("Name", string.Empty));

            HeroEquipment.Add((EquipmentSlot)obj.GetInt("Slot"), item);
        }

        // содержимое карманов
        JsonArray pocketItems = json.Get<JsonArray>("pocket_items");

        for (int i = 0; i < pocketItems.Count; i++)
        {
            if (pocketItems[i] == null)
                continue;

            MaterialData itemData = MaterialsDataStorage.Instance.GetByName((string)pocketItems[i]);

            PocketItems[i] = new MaterialInfo(itemData, 1);
        }

        // прогресс по миссиям

        NormalWorldMissionNumber = json.GetInt("Normal");
        WaterWorldMissionNumber = json.GetInt("Water");
        FireWorldMissionNumber = json.GetInt("Fire");
        EarthWorldMissionNumber = json.GetInt("Earth");
        DarknessWorldMissionNumber = json.GetInt("Darkness");
        AirWorldMissionNumber = json.GetInt("Air");
    }

    ///////////////
    public JsonObject SaveProfile()
    {
        JsonObject profileDataToSave = new JsonObject();

        // сохраняем снаряжение персонажа
        JsonArray heroEquipmentArray = new JsonArray();

        foreach (KeyValuePair<EquipmentSlot, EquipmentData> pair in HeroEquipment)
        {
            heroEquipmentArray.Add(pair.Value);
        }

        profileDataToSave.Add("hero_equipment", heroEquipmentArray);

        // сохраняем содержимое карманов
        JsonArray pocketsItems = new JsonArray();

        for (int i = 0; i < PocketItems.Length; i++)
        {
            if (PocketItems[i] == null)
                continue;

            pocketsItems.Add(PocketItems[i].Data.Name);
        }

        profileDataToSave.Add("pocket_items", pocketsItems);

        // сохраняем прогресс игрока
        profileDataToSave.Add("Normal", NormalWorldMissionNumber);
        profileDataToSave.Add("Water", WaterWorldMissionNumber);
        profileDataToSave.Add("Fire", FireWorldMissionNumber);
        profileDataToSave.Add("Earth", EarthWorldMissionNumber);
        profileDataToSave.Add("Darkness", DarknessWorldMissionNumber);
        profileDataToSave.Add("Air", AirWorldMissionNumber);

        return profileDataToSave;
    }

    ///////////////
    private void InitBaseStats(JsonArray data)
    {
        JsonObject json = data.GetAt<JsonObject>(0);

        Health = json.GetInt("health");
        BaseDamage = json.GetInt("damage");

        // подписка на ивент обновления прогресса
        BattleManager.OnMissionComplete += TryUpdateProgress;
    }
}
