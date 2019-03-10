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
    }

    //////////////
    private void OnDisable()
    {
        BattleManager.OnEnemySpawn -= TryFindNewEnemy;
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
        Debug.Log("Player apply damage " + Strength);
        m_TargetEnemy.TakeDamage(Strength);
    }

    //////////////
    private void TryFindNewEnemy()
    {
        m_TargetEnemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

}
