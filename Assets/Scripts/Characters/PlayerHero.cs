using UnityEngine;

public class PlayerHero : Character
{
    public ElementType HeroAttackType { get; private set; }
    public ElementType HeroDefenseType { get; private set; }

    private Enemy m_TargetEnemy;
    
    //////////////
    private void Start()
    {
        AttackPower = 5;
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
        Debug.Log("Player apply damage " + AttackPower);
        m_TargetEnemy.TakeDamage(AttackPower);
    }

    //////////////
    public void SetEnemy(Enemy enemy)
    {
        m_TargetEnemy = enemy;
    }
}
