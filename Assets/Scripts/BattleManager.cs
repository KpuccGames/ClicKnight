using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    private static BattleManager m_Instance;
    public static BattleManager Instance { get; private set; }

    public PlayerHero m_PlayerHero;

    public Transform m_EnemySpawnPoint;
    public TextMeshProUGUI m_StageCountertext;

    private Enemy m_EnemyCharacter;
    private const float m_EnemySpawnDelay = 1f;

    private static MissionData m_CurrentMission;
    private static int m_CurrentStage;

    public static event Action<MissionData> OnMissionComplete;

    //////////////
    private void OnEnable()
    {
        Enemy.OnEnemyDeath += TryStartNextWave;
        m_PlayerHero.OnPlayerDied += OnPlayerDied;
    }

    //////////////
    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= TryStartNextWave;
        m_PlayerHero.OnPlayerDied -= OnPlayerDied;

        m_CurrentMission = null;
    }

    //////////////
    private void Awake()
    {
        Instance = this;
    }

    //////////////
    private void OnDestroy()
    {
        Instance = null;
    }

    //////////////
    private void Start()
    {
        // инициализация персонажа игрока (из PlayerPrefs?)
        SpawnEnemy();

        RefreshStageCounter();
    }

    //////////////
    private void OnPlayerDied()
    {
        Debug.Log("You lost.");

        SceneManager.LoadScene(SceneName.Home);
    }

    //////////////
    public static void StartMission(MissionData mission)
    {
        m_CurrentMission = mission;
        m_CurrentStage = 1;

        SceneManager.LoadScene(SceneName.BattleScene);
    }

    //////////////
    private void TryStartNextWave(EnemyData enemyData)
    {
        // дропаем предмет игроку
        MaterialData droppedItem = null;

        foreach (MaterialData drop in enemyData.Drops)
        {
            if (Helper.CheckDropEvent(drop.DropChance))
            {
                if (droppedItem == null)
                {
                    droppedItem = drop;
                }
                else if (droppedItem.DropChance > drop.DropChance)
                {
                    droppedItem = drop;
                }
            }
        }

        if (droppedItem != null)
        {
            InventoryContent.Instance.AddMaterial(droppedItem);
            Debug.Log("Dropped item " + droppedItem.Name);
        }

        Debug.Log("Completed stage " + m_CurrentStage);
        m_CurrentStage++;

        if (CheckBattleOver())
        {
            Debug.Log("Mission complete");

            if (OnMissionComplete != null)
                OnMissionComplete(m_CurrentMission);

            SceneManager.LoadScene(SceneName.Home);
            return;
        }

        RefreshStageCounter();
        SpawnEnemy();
    }

    //////////////
    private void RefreshStageCounter()
    {
        m_StageCountertext.text = m_CurrentStage + " / " + m_CurrentMission.Waves;
    }

    //////////////
    public void OnClick()
    {
        if (m_EnemyCharacter == null)
            return;

        // TODO реализация атаки
        m_PlayerHero.Attack();
    }

    //////////////
    private void SpawnEnemy()
    {
        StartCoroutine(SpawningProcess());
    }

    //////////////
    private IEnumerator SpawningProcess()
    {
        m_EnemyCharacter = null;

        int random = UnityEngine.Random.Range(0, m_CurrentMission.Enemies.Count);
        EnemyData enemy = m_CurrentMission.Enemies[random];

        yield return new WaitForSecondsRealtime(m_EnemySpawnDelay);

        m_EnemyCharacter = Instantiate(enemy.GetPrefab(), m_EnemySpawnPoint).GetComponent<Enemy>();
        m_EnemyCharacter.SetupEnemy(enemy);

        m_PlayerHero.SetEnemy(m_EnemyCharacter);

        Debug.Log("Spawned " + m_EnemyCharacter.name);
    }

    //////////////
    private bool CheckBattleOver()
    {
        return m_CurrentStage > m_CurrentMission.Waves;
    }
}
