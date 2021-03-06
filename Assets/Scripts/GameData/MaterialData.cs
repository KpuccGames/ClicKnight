﻿using SimpleJson;
using UnityEngine;

public enum ConsumeEffect
{
    None = 0,
    Heal = 1
}

public class MaterialData
{
    public string Name { get; private set; }
    public string Icon { get; private set; }
    public ConsumeEffect ConsumeEffect { get; private set; }
    public int ConsumeValue { get; private set; }

    ////////////////
    public MaterialData(JsonObject json)
    {
        Name = (string)json["name"];
        Icon = (string)json["icon"];
        ConsumeEffect = Helper.ParseEnum((string)json["consume_effect"], ConsumeEffect.None);
        ConsumeValue = json.GetInt("consume_value", 0);
    }

    /////////////////
    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>(Icon);
    }
}
