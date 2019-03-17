using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;

public class Inventory : MonoBehaviour
{
    public InventoryCell[] m_InventoryCells;

    ////////////////
    void Start()
    {
        string text = File.ReadAllText("Assets/GameData/test_equip.json");
        JsonObject json = Helper.ParseJson(text);

        JsonArray array = json.Get<JsonArray>("items");

        for (int i = 0; i < array.Count; i++)
        {
            JsonObject itemJson = array.GetAt<JsonObject>(i);

            Sprite icon = Resources.Load<Sprite>((string)itemJson["icon"]);

            m_InventoryCells[i].SetItemIcon(icon);
        }
    }
}
