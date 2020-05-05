﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInfo : IItem
{
    public MaterialData Data { get; private set; }
    public int Amount { get; private set; }

    /////////////////
    public MaterialInfo(MaterialData data, int amount)
    {
        Data = data;
        Amount = amount;
    }

    /////////////////
    public void AddMaterial(int amount)
    {
        Amount += amount;
    }

    /////////////////
    public bool TryRemoveMaterial(int amount)
    {
        if (amount > Amount)
            return false;

        Amount -= amount;

        return true;
    }

    ////////////////
    public ItemType GetItemType()
    {
        return ItemType.Material;
    }

    ////////////////
    public void ApplyConsumeEffect(Character hero)
    {
        switch (Data.ConsumeEffect)
        {
            case ConsumeEffect.Heal:
                hero.ApplyHeal(Data.ConsumeValue);
                break;
        }
    }

    public bool IsConsumable()
    {
        return Data.ConsumeEffect != ConsumeEffect.None;
    }
}