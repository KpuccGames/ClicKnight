using System.Collections;
using System.Collections.Generic;

public enum EquipmentSlot
{
    helmet = 0,
    chest = 1,
    legs = 2,
    boots = 3,
    shoulders = 4,
    gloves = 5,
    neck = 6,
    finger = 7
}

public class EquipmentItem
{
    public int AttackBonus { get; private set; }
    public int DefenseBonus { get; private set; }
}
