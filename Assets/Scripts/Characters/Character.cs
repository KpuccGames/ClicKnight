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
    public int Armor { get; protected set; }
    public float CriticalAttackChance { get; protected set; }

    //////////////
    public virtual void Attack()
    { }

    //////////////
    public virtual void TakeDamage(int damage)
    { }
}
