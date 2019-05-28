using System.Collections;
using System;
using UnityEngine;

public class Enemy : Character
{
    public ElementType EnemyType { get; private set; }

    private EnemyData m_EnemyData;

    public static event Action OnEnemyDeath;

    //////////////
    public void SetupEnemy(EnemyData data)
    {
        m_EnemyData = data;

        Health = m_EnemyData.Health;
        AttackPower = m_EnemyData.Damage;
        CriticalAttackChance = 0;
        Armor = 0;

        StartCoroutine(StartAttacking());
    }

    //////////////
    public override void Attack()
    {
        Debug.Log("Enemy attacks " + AttackPower);
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
            if (OnEnemyDeath != null)
                OnEnemyDeath();

            Debug.Log("Enemy is dead");

            Destroy(gameObject);
        }
    }

    //////////////
    public IEnumerator StartAttacking()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(m_EnemyData.AttackRate);

            Attack();
        }
    }
}
