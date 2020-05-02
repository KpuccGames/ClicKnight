using SimpleJson;
using System.Collections.Generic;
using UnityEngine;

public enum MissionWorld
{
    Normal = 0,
    Water = 1,
    Fire = 2,
    Air = 3,
    Earth = 4,
    Darkness = 5
}

public class MissionData : IDataStorageObject
{
    public string Name { get; private set; }
    public int Number { get; private set; }
    public List<EnemyData> NormalEnemies { get; private set; }
    public List<EnemyData> EliteEnemies { get; private set; }
    public List<EnemyData> BossEnemies { get; private set; }
    public int Waves { get; private set; }
    public MissionWorld World { get; private set; }

    private int m_ElitePeriod;

    ///////////////
    public void Init(JsonObject json)
    {
        Number = json.GetInt("number");
        // TODO добавить названия миссий (надо ли?)
        Name = $"Mission_{Number}";
        Waves = json.GetInt("waves");
        World = (MissionWorld)json.GetInt("world");
        m_ElitePeriod = json.GetInt("elite_period");

        NormalEnemies = new List<EnemyData>();
        EliteEnemies = new List<EnemyData>();
        BossEnemies = new List<EnemyData>();

        string[] enemiesList = json.GetString("enemies", string.Empty).Split(',');

        foreach (string enemyName in enemiesList)
        {
            EnemyData enemy = EnemiesDataStorage.Instance.GetByName(enemyName);

            if (enemy == null)
            {
                Debug.LogWarning("Null EnemyData in " + World + " mission number: " + Number);
                continue;
            }

            switch (enemy.Type)
            {
                case EnemyType.Normal:
                    NormalEnemies.Add(enemy);
                    break;

                case EnemyType.Elite:
                    EliteEnemies.Add(enemy);
                    break;

                case EnemyType.Boss:
                    BossEnemies.Add(enemy);
                    break;
            }

        }
    }

    ///////////////
    public EnemyData GetEnemy(int stageNum)
    {
        EnemyData enemyToReturn = null;

        if (BossEnemies.Count > 0 && stageNum == Waves)
        {
            enemyToReturn = BossEnemies[0]; // босс  миссии подразумевается только один
        }
        else if (m_ElitePeriod > 0 && stageNum % m_ElitePeriod == 0)
        {
            int rand = Random.Range(0, EliteEnemies.Count);

            enemyToReturn = EliteEnemies[rand];
        }
        else
        {
            int rand = Random.Range(0, NormalEnemies.Count);

            enemyToReturn = NormalEnemies[rand];
        }

        if (enemyToReturn != null)
            return enemyToReturn;
        else // кусок для отслежавния ошибки
        {
            Debug.LogError("Enemy generation error!");

            if (NormalEnemies.Count > 0)
                return NormalEnemies[0];
            else if (EliteEnemies.Count > 0)
                return EliteEnemies[0];
            else
                return BossEnemies[0];
        }
    }
}

public class MissionsDataStorage : BaseDataStorage<MissionData, MissionsDataStorage>
{
    public MissionsDataStorage() : base("missions") { }
}
