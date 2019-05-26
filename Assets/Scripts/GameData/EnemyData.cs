using SimpleJson;
using UnityEngine;

public enum AttackType
{
    common = 0,
    water = 1,
}

public class EnemyData
{
    public string Name { get; private set; }
    public string Prefab { get; private set; }
    public AttackType AttackType { get; private set; }
    public int Damage { get; private set; }
    public int Health { get; private set; }
    public float AttackRate { get; private set; }

    ///////////////
    public EnemyData(JsonObject json)
    {
        Name = (string)json["name"];
        Prefab = (string)json["prefab"];
        AttackType = (AttackType)json.GetInt("attack_type", 0);
        Damage = json.GetInt("damage");
        Health = json.GetInt("health");
        AttackRate = json.GetFloat("attack_rate");
    }

    ///////////////
    public GameObject GetPrefab()
    {
        return Resources.Load<GameObject>(Prefab);
    }
}
