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

    [Header("Ability evade")]
    public RectTransform m_TapEvadePrefabsPlaceMask;
    public AbilityTapEvadePrefab m_TapPrefab;

    [Header("Pockets")]
    public BattlePocket[] m_Pockets;

    private Enemy m_EnemyCharacter;
    private Coroutine m_AbilityCastingCoroutine;

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
        SpawnEnemy();

        // инициализация содержимого карманов
        for (int i = 0; i < m_Pockets.Length; i++)
        {
            m_Pockets[i].SetupPocket(PlayerProfile.Instance.PocketItems[i], m_PlayerHero);
        }

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
    private void TryStartNextWave()
    {
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
        // TODO переделать реализацию, чтобы не приходилось уничтожать объект и создавать заново

        m_EnemyCharacter = null;

        EnemyData enemy = m_CurrentMission.GetEnemy(m_CurrentStage);

        yield return new WaitForSecondsRealtime(m_EnemySpawnDelay);

        m_EnemyCharacter = Instantiate(enemy.GetPrefab(), m_EnemySpawnPoint).GetComponent<Enemy>();
        m_EnemyCharacter.SetupEnemy(enemy);

        m_PlayerHero.SetEnemy(m_EnemyCharacter);

        Debug.Log("Spawned " + m_EnemyCharacter.name);
    }

    //////////////
    public void CreateAbilityCastObject(int damage, float reactionTime)
    {
        AbilityTapEvadePrefab evadeItem = Instantiate(m_TapPrefab, m_TapEvadePrefabsPlaceMask);
        evadeItem.Setup(damage, reactionTime);

        float width = m_TapEvadePrefabsPlaceMask.rect.width / 2;
        float height = m_TapEvadePrefabsPlaceMask.rect.height / 2;

        float itemPosX = UnityEngine.Random.Range(-width, width);
        float itemPosY = UnityEngine.Random.Range(-height, height);

        evadeItem.GetComponent<RectTransform>().localPosition = new Vector2(itemPosX, itemPosY);
    }

    //////////////
    private bool CheckBattleOver()
    {
        return m_CurrentStage > m_CurrentMission.Waves;
    }
}
