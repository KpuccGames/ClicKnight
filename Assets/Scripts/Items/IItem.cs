using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    equipment = 0,
    material = 1
}

public interface IItem
{
    Sprite GetIcon();
    ItemType GetItemType();
}
