﻿using UnityEngine;

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
    public int Strength { get; private set; }
    public int Health { get; private set; }
    public int Defence { get; private set; }
    public float CriticalAttackChance { get; private set; }
}