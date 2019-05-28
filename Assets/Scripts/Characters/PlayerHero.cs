using UnityEngine;
using System.Collections.Generic;

public class PlayerHero : Character
{
    public ElementType HeroAttackType { get; private set; }
    public ElementType HeroDefenseType { get; private set; }

    private Dictionary<string, EquipmentItem> m_Equipment;

    private Enemy m_TargetEnemy;
    
    //////////////
    private void Start()
    {
        // setup персонажа игрока
        AttackPower = PlayerProfile.Instance.GetDamage();
        Health = PlayerProfile.Instance.Health;
        Armor = PlayerProfile.Instance.GetArmor();
    }

    //////////////
    public override void TakeDamage(int damage)
    {
        int calculatedDamage = Armor - damage;

        //
        // NOTE: сначала обработка входящего дамага, потом базовое применение
        //

        if (calculatedDamage > 0)
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
        Debug.Log("Player apply damage " + AttackPower);
        m_TargetEnemy.TakeDamage(AttackPower);
    }

    //////////////
    public void SetEnemy(Enemy enemy)
    {
        m_TargetEnemy = enemy;
    }
}
