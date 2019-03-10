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
    public IEnumerator StartAttacking()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(AttackRate);

            Attack();
        }
    }
}
