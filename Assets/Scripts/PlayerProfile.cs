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
    public Dictionary<EquipmentSlot, EquipmentItem> HeroEquipment { get; private set; } = new Dictionary<EquipmentSlot, EquipmentItem>();
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

        HeroEquipment = new Dictionary<EquipmentSlot, EquipmentItem>();
    }

    ///////////////
    public int GetDamage()
    {
        int damage = BaseDamage;

        foreach (EquipmentItem equipment in HeroEquipment.Values)
        {
            damage += equipment.AttackBonus;
        }

        return damage;
    }

    ///////////////
    public int GetHealth()
    {
        int health = Health;

        foreach (EquipmentItem equipment in HeroEquipment.Values)
        {
            health += equipment.HealthBonus;
        }

        return health;
    }

    ///////////////
    public void EquipItem(EquipmentItem item)
    {
        if (item == null)
            return;

        EquipmentItem equippedItem;

        // если на персонаже нет предмета в этом слоте, тогда надеваем предмет
        if (!HeroEquipment.TryGetValue(item.Slot, out equippedItem))
        {
            HeroEquipment.Add(item.Slot, item);
            InventoryContent.Instance.RemoveItem(item);

            if (OnEquipmentChanged != null)
                OnEquipmentChanged();

            return;
        }

        InventoryContent.Instance.AddEquipmentItem(equippedItem);
        InventoryContent.Instance.RemoveItem(item);
        HeroEquipment[item.Slot] = item;

        if (OnEquipmentChanged != null)
            OnEquipmentChanged();
    }

    ///////////////
    public void UnequipItem(EquipmentItem item)
    {
        if (item == null)
            return;

        HeroEquipment.Remove(item.Slot);
        InventoryContent.Instance.AddEquipmentItem(item);

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
                            InventoryContent.Instance.AddEquipmentItem("axe");
                            break;

                        case 3:
                            InventoryContent.Instance.AddMaterial("ingot", 15);
                            break;
                    }
                }
                break;
        }

        GameManager.Instance.SaveGame();
    }

    ///////////////
    public void AddItemInPocket(MaterialData data)
    {
        if (!InventoryContent.Instance.TryRemoveMaterial(data, 1))
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
    public void RemoveItemFromPocket(int pocketNumber)
    {
        if (PocketItems[pocketNumber] == null)
            return;

        InventoryContent.Instance.AddMaterial(PocketItems[pocketNumber].Data);

        PocketItems[pocketNumber] = null;
    }

    //Utils//

    ///////////////
    public void LoadProfile(JsonObject json)
    {
        InitBaseStats(GameDataStorage.Instance.NewProfileData);

        // экипировка героя
        JsonArray heroEquipments = json.Get<JsonArray>("hero_equipment");

        foreach (JsonObject obj in heroEquipments)
        {
            EquipmentItem item = GameDataStorage.Instance.GetEquipmentByName(obj.GetString("Name", string.Empty));

            HeroEquipment.Add((EquipmentSlot)obj.GetInt("Slot"), item);
        }

        // содержимое карманов
        JsonArray pocketItems = json.Get<JsonArray>("pocket_items");

        for (int i = 0; i < pocketItems.Count; i++)
        {
            if (pocketItems[i] == null)
                continue;

            MaterialData itemData = GameDataStorage.Instance.GetMaterialByName((string)pocketItems[i]);

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

        foreach (KeyValuePair<EquipmentSlot, EquipmentItem> pair in HeroEquipment)
        {
            heroEquipmentArray.Add(pair.Value);
        }

        profileDataToSave.Add("hero_equipment", heroEquipmentArray);

        // сохраняем содержимое карманов
        JsonArray pocketsItems = new JsonArray();

        for (int i = 0; i < PocketItems.Length; i++)
        {
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
