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

    ///////////////
    public void Init()
    {
        string text = File.ReadAllText("Assets/GameData/GameData.json");
        JsonObject json = Helper.ParseJson(text);

        // init equipment
        Equipments = new List<EquipmentItem>();

        JsonArray equipments = json.Get<JsonArray>("equipments");

        foreach (JsonObject obj in equipments)
        {
            Equipments.Add(new EquipmentItem(obj));
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
}
