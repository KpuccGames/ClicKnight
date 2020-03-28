using UnityEngine;
using System;
using TMPro;

public class PlayerHero : Character
{
    [Header("Canvas")]
    public TextMeshProUGUI m_PlayerHealth;

    private Enemy m_TargetEnemy;

    public event Action OnPlayerDied;

    //////////////
    private void OnEnable()
    {
        OnHealApplied += UpdateHeroHealthText;
    }

    //////////////
    private void OnDisable()
    {
        OnHealApplied -= UpdateHeroHealthText;
    }

    //////////////
    private void Start()
    {
        // setup персонажа игрока
        AttackPower = PlayerProfile.Instance.GetDamage();
        Health = PlayerProfile.Instance.GetHealth();
        m_MaxHealth = Health;

        m_PlayerHealth.text = Health.ToString();
    }

    //////////////
    public override void TakeDamage(int damage)
    {
        Debug.Log("Enemy attacks " + damage);

        //
        // NOTE: сначала обработка входящего дамага, потом базовое применение
        //

        if (damage > 0)
            Health -= damage;

        if (Health < 0)
            Health = 0;

        UpdateHeroHealthText();

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

    //////////////
    private void UpdateHeroHealthText()
    {
        m_PlayerHealth.text = Health.ToString();
    }
}
