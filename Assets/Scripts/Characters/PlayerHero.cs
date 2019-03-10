using UnityEngine;

public class PlayerHero : Character
{
    public ElementType HeroAttackType { get; private set; }
    public ElementType HeroDefenseType { get; private set; }

    private Enemy m_TargetEnemy;

    //////////////
    private void Start()
    {
        Strength = 5;
        Health = 20;

        TryFindNewEnemy();
    }

    //////////////
    public override void TakeDamage(int damage)
    {
        int calculatedDamage = damage;

        //
        // NOTE: сначала обработка входящего дамага, потом базовое применение
        //

        base.TakeDamage(calculatedDamage);
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
        if (m_TargetEnemy != null)
            return;

        m_TargetEnemy = GameObject.Find("Enemy").GetComponent<Enemy>();
    }

}
