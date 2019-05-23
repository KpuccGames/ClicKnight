using SimpleJson;
using System.Collections.Generic;

public class MissionData
{
    public int Number { get; private set; }
    public List<EnemyData> Enemies { get; private set; }
    public int Waves { get; private set; }

    ///////////////
    public MissionData(JsonObject json)
    {
        Number = json.GetInt("number");
        Waves = json.GetInt("waves");

        Enemies = new List<EnemyData>();

        string[] enemiesList = json.GetString("enemies", string.Empty).Split(',');

        foreach (string enemyName in enemiesList)
        {
            Enemies.Add(GameDataStorage.Instance.GetEnemyByName(enemyName));
        }
    }
}
