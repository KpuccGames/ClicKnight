using System.Collections;
using System;
using UnityEngine;
using TMPro;

public class Enemy : Character
{
    [Header("Canvas")]
    public TextMeshProUGUI m_EnemyHealthText;

    private EnemyData m_EnemyData;
    private PlayerHero m_PlayerHero;

    private bool m_IsCastingAbility;
    private DateTime m_AbilityCastedLastTime = DateTime.MinValue;
    private Coroutine m_AbilityCastingProcess;

    public static event Action OnEnemyDeath;

    //////////////
    public void SetupEnemy(EnemyData data)
    {
        m_EnemyData = data;

        Health = m_EnemyData.Health;
        AttackPower = m_EnemyData.Damage;
        CriticalAttackChance = 0;

        m_EnemyHealthText.text = Health.ToString();

        m_PlayerHero = BattleManager.Instance.m_PlayerHero;

        StartCoroutine(StartAttacking());
    }

    //////////////
    public override void Attack()
    {
        TryStartCastAbility();

        if (m_IsCastingAbility)
            return;

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

            if (m_AbilityCastingProcess != null)
            {
                StopCoroutine(m_AbilityCastingProcess);
                m_AbilityCastingProcess = null;
            }

            // дропаем предмет игроку
            MaterialData droppedItem = m_EnemyData.TryDropItem();

            if (droppedItem != null)
            {
                InventoryContent.Instance.AddMaterial(droppedItem);
                Debug.Log("Dropped item " + droppedItem.Name);
            }
            
            OnEnemyDeath?.Invoke();

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

    //////////////
    private void TryStartCastAbility()
    {
        if (IsAbilityOnCooldown())
            return;

        m_IsCastingAbility = true;

        m_AbilityCastingProcess = StartCoroutine(StartCastAbility());
        m_AbilityCastedLastTime = DateTime.UtcNow;
    }

    //////////////
    private IEnumerator StartCastAbility()
    {
        AbilityData ability = GameDataStorage.Instance.GetAbilityByName(m_EnemyData.Ability);

        for (int i = 0; i < ability.Strikes.Length; i++)
        {
            BattleManager.Instance.CreateAbilityCastObject(ability.Strikes[i], ability.ReactionTime);

            yield return new WaitForSecondsRealtime(ability.ReactionTime);
        }

        m_IsCastingAbility = false;
    }

    //////////////
    private bool IsAbilityOnCooldown()
    {
        AbilityData ability = GameDataStorage.Instance.GetAbilityByName(m_EnemyData.Ability);

        if (ability == null)
            return true;

        TimeSpan dt = TimeSpan.FromSeconds(ability.Cooldown);

        return m_AbilityCastedLastTime + dt > DateTime.UtcNow;
    }
}
