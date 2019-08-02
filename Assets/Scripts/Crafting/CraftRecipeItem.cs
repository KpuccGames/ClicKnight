using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftRecipeItem : MonoBehaviour
{
    public Image m_Icon;

    private CraftingData m_Recipe;

    public static event Action<CraftingData> OnClickRecipe;

    ///////////////
    public void Init(CraftingData data)
    {
        m_Recipe = data;

        EquipmentItem item = GameDataStorage.Instance.GetEquipmentByName(m_Recipe.m_CraftItem);

        m_Icon.overrideSprite = item.GetIcon();
    }

    ///////////////
    public void OnClick()
    {
        OnClickRecipe?.Invoke(m_Recipe);
    }
}
