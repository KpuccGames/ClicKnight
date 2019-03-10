using UnityEngine;

public enum ElementType
{
    Unknown = -1,
    Ice = 0,
    Fire = 1,
    Air = 2,
    Earth = 3,
    Light = 4,
    Darkness = 5,
}

public class Character : MonoBehaviour
{
    public int Strength { get; protected set; }
    public int Health { get; protected set; }
    public int Defence { get; protected set; }
    public float CriticalAttackChance { get; protected set; }

    //////////////
    public virtual void Attack()
    {

    }
}
