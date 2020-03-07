using UnityEngine;

public enum ElementType
{
    Common = 0,
    Ice = 1,
    Fire = 2,
    Air = 3,
    Earth = 4
}

public class Character : MonoBehaviour
{
    public int AttackPower { get; protected set; }
    public int Health { get; protected set; }
    public float CriticalAttackChance { get; protected set; }

    public ElementType AttackType { get; private set; }
    public ElementType DefenseType { get; private set; }

    protected int m_MaxHealth;

    //////////////
    public virtual void Attack()
    { }

    //////////////
    public virtual void TakeDamage(int damage)
    { }

    //////////////
    public int GetDamageMultiplier()
    {
        // в аргументах будет атакующий и защищающийся юнит

        return 1;
    }

    //////////////
    public void ApplyHeal(int value)
    {
        Health += value;

        if (Health > m_MaxHealth)
            Health = m_MaxHealth;
    }
}
