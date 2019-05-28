using System.Collections;
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
    public Dictionary<string, EquipmentItem> Equipment { get; private set; }

    public int NormalWorldMissionNumber { get; private set; }
    public int FireWorldMissionNumber { get; private set; }
    public int WaterWorldMissionNumber { get; private set; }
    public int AirWorldMissionNumber { get; private set; }
    public int EarthWorldMissionNumber { get; private set; }
    public int DarknessWorldMissionNumber { get; private set; }

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

        Equipment = new Dictionary<string, EquipmentItem>();

        string equipName = (string)json["base_weapon"];
        Equipment.Add(equipName, GameDataStorage.Instance.GetEquipmentByName(equipName));
    }

    ///////////////
    public int GetDamage()
    {
        int damage = BaseDamage;

        foreach (EquipmentItem equipment in Equipment.Values)
        {
            damage += equipment.AttackBonus;
        }

        return damage;
    }

    ///////////////
    public int GetArmor()
    {
        int armor = Armor;

        foreach (string equipmentName in Equipment)
        {
            armor += GameDataStorage.Instance.GetEquipmentByName(equipmentName).ArmorBonus;
        }

        return armor;
    }

    ///////////////
    public void LoadProfile(JsonObject json)
    {
        // загружаем профайл из сохраненных данных
    }
}
