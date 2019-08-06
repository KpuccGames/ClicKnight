﻿using SimpleJson;
using UnityEngine;
using System.Collections.Generic;

public enum AttackType
{
    common = 0,
    water = 1,
}

public enum EnemyType
{
    Normal = 0,
    Elite = 1,
    Boss = 2
}

public class EnemyData
{
    public string Name { get; private set; }
    public string Prefab { get; private set; }
    public AttackType AttackType { get; private set; }
    public EnemyType Type { get; private set; }
    public int Damage { get; private set; }
    public int Health { get; private set; }
    public float AttackRate { get; private set; }
    public List<MaterialData> Drops { get; private set; }
    public string Ability { get; private set; }

    ///////////////
    public EnemyData(JsonObject json)
    {
        Name = (string)json["name"];
        Prefab = (string)json["prefab"];
        AttackType = (AttackType)json.GetInt("attack_type", 0);
        Damage = json.GetInt("damage");
        Health = json.GetInt("health");
        AttackRate = json.GetFloat("attack_rate");
        Type = (EnemyType)json.GetInt("type");

        Drops = new List<MaterialData>();
        string[] dropList = json.GetString("drops", string.Empty).Split(',');

        foreach (string dropItem in dropList)
        {
            Drops.Add(GameDataStorage.Instance.GetMaterialByName(dropItem));
        }

        Ability = (string)json["ability"];
    }

    ///////////////
    public GameObject GetPrefab()
    {
        return Resources.Load<GameObject>(Prefab);
    }
}
