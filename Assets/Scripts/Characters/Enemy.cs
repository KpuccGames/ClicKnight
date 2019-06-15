using System.Collections;
using System;
using UnityEngine;
using TMPro;

public class Enemy : Character
{
    [Header("Canvas")]
    public TextMeshProUGUI m_EnemyHealthText;

    public ElementType EnemyType { get; private set; }

    private EnemyData m_EnemyData;
    private PlayerHero m_PlayerHero;

    public static event Action OnEnemyDeath;

    //////////////
    public void SetupEnemy(EnemyData data)
    {
        m_EnemyData = data;

        Health = m_EnemyData.Health;
        AttackPower = m_EnemyData.Damage;
        CriticalAttackChance = 0;
        Armor = 0;

        m_EnemyHealthText.text = Health.ToString();

        m_PlayerHero = BattleManager.Instance.m_PlayerHero;

        StartCoroutine(StartAttacking());
    }

    //////////////
    public override void Attack()
    {
        m_PlayerHero.TakeDamage(AttackPower);
    }

    //////////////
    public override void TakeDamage(int damage)
    {
        int calculatedDamage = damage;

        //
        // NOTE: сначала обработка входящего дамага, потом базовое применение
        //

        if (calculatedDamage > 0)
            Health -= calculatedDamage;

        if (Health < 0)
            Health = 0;

        m_EnemyHealthText.text = Health.ToString();

        // после применения урона проверяем, не погиб ли персонаж
        if (Health <= 0)
        {
            Debug.Log(m_EnemyData.Name + " is dead");

            if (OnEnemyDeath != null)
                OnEnemyDeath();

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
