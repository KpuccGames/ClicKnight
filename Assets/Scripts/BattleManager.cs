using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private PlayerHero m_PlayerHero;
    private Enemy m_EnemyCharacter;
    private const float m_EnemySpawnDelay = 1f;

    public Transform m_EnemySpawnPoint;
    public Enemy m_EnemyPrefab;

    public static event Action OnEnemySpawn;

    //////////////
    private void OnEnable()
    {
        Enemy.OnEnemyDeath += SpawnEnemy;
    }

    //////////////
    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= SpawnEnemy;
    }

    //////////////
    private void Start()
    {
        m_PlayerHero = GameObject.Find("PlayerHero").GetComponent<PlayerHero>();

        SpawnEnemy();
    }

    //////////////
    public void OnClick()
    {
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

        yield return new WaitForSecondsRealtime(m_EnemySpawnDelay);

        m_EnemyCharacter = Instantiate(m_EnemyPrefab, m_EnemySpawnPoint);

        if (OnEnemySpawn != null)
            OnEnemySpawn();
    }
}
