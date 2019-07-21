using UnityEngine;
using System;
using TMPro;

public class PlayerHero : Character
{
    [Header("Canvas")]
    public TextMeshProUGUI m_PlayerHealth;

    public ElementType HeroAttackType { get; private set; }
    public ElementType HeroDefenseType { get; private set; }

    private Enemy m_TargetEnemy;

    public event Action OnPlayerDied;
    
    //////////////
    private void Start()
    {
        // setup персонажа игрока
        AttackPower = PlayerProfile.Instance.GetDamage();
        Health = PlayerProfile.Instance.Health;
        Armor = PlayerProfile.Instance.GetArmor();

        m_PlayerHealth.text = Health.ToString();
    }

    //////////////
    public override void TakeDamage(int damage)
    {
        int calculatedDamage = damage - Armor;

        Debug.Log("Enemy attacks " + calculatedDamage);

        //
        // NOTE: сначала обработка входящего дамага, потом базовое применение
        //

        if (calculatedDamage > 0)
            Health -= calculatedDamage;

        if (Health < 0)
            Health = 0;

        m_PlayerHealth.text = Health.ToString();

        // после применения урона проверяем, не погиб ли персонаж
        if (Health <= 0)
        {
            Destroy(gameObject);

            if (OnPlayerDied != null)
                OnPlayerDied();
        }
    }

    //////////////
    public override void Attack()
    {
        m_TargetEnemy.TakeDamage(AttackPower);
    }

    //////////////
    public void SetEnemy(Enemy enemy)
    {
        m_TargetEnemy = enemy;
    }
}
