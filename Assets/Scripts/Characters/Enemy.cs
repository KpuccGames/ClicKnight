using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    public ElementType EnemyType { get; private set; }
    public float AttackRate { get; private set; }

    //////////////
    private void Start()
    {
        Health = 10;
        Strength = 2;
        AttackRate = 2f;

        StartCoroutine(StartAttacking());
    }

    //////////////
    public override void Attack()
    {
        Debug.Log("Enemy attacks " + Strength);
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
    public IEnumerator StartAttacking()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(AttackRate);

            Attack();
        }
    }
}
