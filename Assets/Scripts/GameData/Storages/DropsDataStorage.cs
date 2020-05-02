using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class DropData : IDataStorageObject
{
    public string Name { get; private set; }
    public string MaterialDropName { get; private set; }
    public float MaterialDropChance { get; private set; }

    ////////////////
    public void Init(JsonObject json)
    {
        Name = (string)json["drop_name"];
        MaterialDropName = (string)json["material_name"];
        MaterialDropChance = json.GetFloat("drop_chance");
    }

    ////////////////
    public bool IsDropped()
    {
        return Helper.CheckDropEvent(MaterialDropChance);
    }
}

public class DropsDataStorage : BaseDataStorage<DropData, DropsDataStorage>
{
    public DropsDataStorage() : base("drops") { }
}
