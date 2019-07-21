using SimpleJson;
using System.Collections.Generic;

public enum MissionWorld
{
    Normal = 0,
    Water = 1,
    Fire = 2,
    Air = 3,
    Earth = 4,
    Darkness = 5
}

public class MissionData
{
    public int Number { get; private set; }
    public List<EnemyData> Enemies { get; private set; }
    public int Waves { get; private set; }
    public MissionWorld World { get; private set; }

    ///////////////
    public MissionData(JsonObject json)
    {
        Number = json.GetInt("number");
        Waves = json.GetInt("waves");
        World = (MissionWorld)json.GetInt("world");

        Enemies = new List<EnemyData>();

        string[] enemiesList = json.GetString("enemies", string.Empty).Split(',');

        foreach (string enemyName in enemiesList)
        {
            Enemies.Add(GameDataStorage.Instance.GetEnemyByName(enemyName));
        }
    }
}
