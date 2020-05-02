using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class AbilityData : IDataStorageObject
{
    public string Name { get; private set; }
    public float ReactionTime { get; private set; }
    public float Cooldown { get; private set; }
    public int[] Strikes { get; private set; }

    public void Init(JsonObject json)
    {
        Name = (string)json["name"];
        ReactionTime = json.GetFloat("reaction_time");
        Cooldown = json.GetFloat("cooldown");

        string[] strikes = json.GetString("strikes", string.Empty).Split(',');

        Strikes = new int[strikes.Length];

        for (int i = 0; i < Strikes.Length; i++)
        {
            Strikes[i] = Convert.ToInt32(strikes[i]);
        }
    }
}

public class AbilitiesDataStorage : BaseDataStorage<AbilityData, AbilitiesDataStorage>
{
    public AbilitiesDataStorage() : base("abilities") { }
}
