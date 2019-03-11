using UnityEngine;

public class PlayerHero : Character
{
    public ElementType HeroAttackType { get; private set; }
    public ElementType HeroDefenseType { get; private set; }

    private Enemy m_TargetEnemy;

    //////////////
    private void OnEnable()
    {
        BattleManager.OnEnemySpawn += TryFindNewEnemy;
        Enemy.OnEnemyDeath += OnEnemyKilled;
    }

    //////////////
    private void OnDisable()
    {
        BattleManager.OnEnemySpawn -= TryFindNewEnemy;
        Enemy.OnEnemyDeath -= OnEnemyKilled;
    }

    //////////////
    private void Start()
    {
        Strength = 5;
        Health = 20;
    }

    //////////////
    public override void TakeDamage(int damage)
    {
        int calculatedDamage = damage;

        //
        // NOTE: сначала обработка входящего дамага, потом базовое применение
        //

        Health -= calculatedDamage;

        // после применения урона проверяем, не погиб ли персонаж
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    //////////////
    public override void Attack()
    {
        if (m_TargetEnemy == null)
        {
            TryFindNewEnemy();
            return;
        }

        Debug.Log("Player apply damage " + Strength);
        m_TargetEnemy.TakeDamage(Strength);
    }

    //////////////
    private void OnEnemyKilled()
    {
        m_TargetEnemy = null;
    }

    //////////////
    private void TryFindNewEnemy()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Enemy");

        if (go != null)
            m_TargetEnemy = go.GetComponent<Enemy>();
    }
}
