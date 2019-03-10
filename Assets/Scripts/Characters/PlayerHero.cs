using UnityEngine;

public class PlayerHero : Character
{
    public ElementType HeroAttackType { get; private set; }
    public ElementType HeroDefenseType { get; private set; }

    //////////////
    private void Start()
    {
        Strength = 5;
        Health = 20;
    }

    //////////////
    public override void Attack()
    {
        Debug.Log("Player apply damage " + Strength);
    }
}
