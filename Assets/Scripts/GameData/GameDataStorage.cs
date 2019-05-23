using SimpleJson;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GameDataStorage
{
    private static GameDataStorage m_Instance;
    public static GameDataStorage Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new GameDataStorage();

            return m_Instance;
        }
    }

    public bool IsInited { get; private set; }

    public List<EquipmentItem> Equipments { get; private set; }
    public List<EnemyData> Enemies { get; private set; }
    public List<MissionData> Missions { get; private set; }
    public List<MaterialData> Materials { get; private set; }

    ///////////////
    public void Init()
    {
        GameDataContainer gameDatas = Resources.Load<GameDataContainer>("GameDataContainer");
        
        foreach (TextAsset text in gameDatas.m_GameDataFiles)
        {
            JsonArray dataArray = Helper.ParseJsonArray(text.ToString());

            switch (text.name)
            {
                case "enemies":
                    Enemies = new List<EnemyData>();

                    foreach (JsonObject obj in dataArray)
                    {
                        Enemies.Add(new EnemyData(obj));
                    }
                    break;

                case "equipments":
                    Equipments = new List<EquipmentItem>();

                    foreach (JsonObject obj in dataArray)
                    {
                        Equipments.Add(new EquipmentItem(obj));
                    }
                    break;

                case "materials":
                    Materials = new List<MaterialData>();

                    foreach (JsonObject obj in dataArray)
                    {
                        Materials.Add(new MaterialData(obj));
                    }
                    break;

                case "missions":
                    Missions = new List<MissionData>();

                    foreach (JsonObject obj in dataArray)
                    {
                        Missions.Add(new MissionData(obj));
                    }
                    break;

                default:
                    Debug.LogError("Wrong storage name");
                    break;
            }
        }

        // end init
        IsInited = true;
    }

    ///////////////
    public EquipmentItem GetEquipmentByName(string name)
    {
        foreach (EquipmentItem item in Equipments)
        {
            if (item.Name == name)
                return item;
        }

        return null;
    }

    ///////////////
    public EnemyData GetEnemyByName(string name)
    {
        foreach (EnemyData enemy in Enemies)
        {
            if (enemy.Name == name)
                return enemy;
        }

        return null;
    }
}
