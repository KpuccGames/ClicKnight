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
    public int Armor { get; private set; }
    public Dictionary<EquipmentSlot, EquipmentItem> HeroEquipment { get; private set; }

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

        string equipName = (string)json["base_weapon"];
        EquipmentItem equip = GameDataStorage.Instance.GetEquipmentByName(equipName);
        HeroEquipment.Add(equip.Slot, equip);
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
    public int GetArmor()
    {
        int armor = Armor;

        foreach (EquipmentItem equipment in HeroEquipment.Values)
        {
            armor += equipment.ArmorBonus;
        }

        return armor;
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
                if (completedMission.Number == NormalWorldMissionNumber)
                    NormalWorldMissionNumber++;
                break;
        }

        GameManager.Instance.SaveGame();
    }

    //Utils//

    ///////////////
    public void LoadProfile(JsonObject json)
    {
        InitBaseStats(GameDataStorage.Instance.NewProfileData);
        HeroEquipment = new Dictionary<EquipmentSlot, EquipmentItem>();

        // загружаем данные сохраненного профиля

        JsonArray heroEquipments = json.Get<JsonArray>("hero_equipment");

        foreach (JsonObject obj in heroEquipments)
        {
            EquipmentItem item = GameDataStorage.Instance.GetEquipmentByName(obj.GetString("Name", string.Empty));

            HeroEquipment.Add((EquipmentSlot)obj.GetInt("Slot"), item);
        }

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
        Armor = json.GetInt("armor");

        // подписка на ивент обновления прогресса
        BattleManager.OnMissionComplete += TryUpdateProgress;
    }
}
