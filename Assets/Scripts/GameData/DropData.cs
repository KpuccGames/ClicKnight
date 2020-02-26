using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class DropData
{
    public string DropName { get; private set; }
    public string MaterialDropName { get; private set; }
    public float MaterialDropChance { get; private set; }

    ////////////////
    public DropData(JsonObject json)
    {
        DropName = (string)json["drop_name"];
        MaterialDropName = (string)json["material_name"];
        MaterialDropChance = json.GetFloat("drop_chance");
    }

    ////////////////
    public bool IsDropped()
    {
        return Helper.CheckDropEvent(MaterialDropChance);
    }
}
