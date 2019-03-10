using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private PlayerHero m_PlayerHero;

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
        Instantiate(m_EnemyPrefab, m_EnemySpawnPoint);

        if (OnEnemySpawn != null)
            OnEnemySpawn();
    }
}
