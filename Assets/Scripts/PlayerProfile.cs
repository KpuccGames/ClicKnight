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

        Health = json.GetInt("health");
        BaseDamage = json.GetInt("damage");
        Armor = json.GetInt("armor");
        NormalWorldMissionNumber = json.GetInt("normal_world_start_mission");
        FireWorldMissionNumber = json.GetInt("fire_world_start_mission");
        WaterWorldMissionNumber = json.GetInt("water_world_start_mission");
        AirWorldMissionNumber = json.GetInt("air_world_start_mission");
        EarthWorldMissionNumber = json.GetInt("earth_world_start_mission");
        DarknessWorldMissionNumber = json.GetInt("darkness_world_start_mission");

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
    public void LoadProfile(JsonObject json)
    {
        // загружаем профайл из сохраненных данных
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

            Debug.Log("Health " + Health);
            Debug.Log("Damage " + GetDamage());
            Debug.Log("Armor " + GetArmor());

            return;
        }

        InventoryContent.Instance.AddItem(equippedItem);
        InventoryContent.Instance.RemoveItem(item);
        HeroEquipment[item.Slot] = item;

        if (OnEquipmentChanged != null)
            OnEquipmentChanged();

        Debug.Log("Health " + Health);
        Debug.Log("Damage " + GetDamage());
        Debug.Log("Armor " + GetArmor());
    }

    ///////////////
    public void UnequipItem(EquipmentItem item)
    {
        if (item == null)
            return;

        HeroEquipment.Remove(item.Slot);
        InventoryContent.Instance.AddItem(item);

        if (OnEquipmentChanged != null)
            OnEquipmentChanged();

        Debug.Log("Health " + Health);
        Debug.Log("Damage " + GetDamage());
        Debug.Log("Armor " + GetArmor());
    }
}
