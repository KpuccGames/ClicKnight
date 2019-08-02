﻿using SimpleJson;
using UnityEngine;

public class MaterialData
{
    public string Name { get; private set; }
    public float DropChance { get; private set; }
    public string Icon { get; private set; }

    ////////////////
    public MaterialData(JsonObject json)
    {
        Name = (string)json["name"];
        DropChance = json.GetFloat("drop_chance", -1);
        Icon = (string)json["icon"];
    }

    /////////////////
    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>(Icon);
    }
}
