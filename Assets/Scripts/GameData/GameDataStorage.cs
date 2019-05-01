using SimpleJson;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GameDataStorage : MonoBehaviour
{
    public static GameDataStorage Instance { get; private set; }

    public bool IsInited { get; private set; }

    public List<EquipmentItem> Equipments { get; private set; }

    ///////////////
    void Start()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(Instance);

        Instance.Init();
    }

    ///////////////
    private void Init()
    {
        string text = File.ReadAllText("Assets/GameData/GameData.json");
        JsonObject json = Helper.ParseJson(text);

        // init equipment
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
