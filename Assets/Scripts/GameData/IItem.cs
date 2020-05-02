using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment = 0,
    Material = 1
}

public interface IItem
{
    ItemType GetItemType();
}
