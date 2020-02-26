using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingDialog : BaseDialog
{
    public CraftRecipeItem m_RecipePrefab;
    public RectTransform m_RecipesRect;
    public Image m_ItemResultIcon;
    public Image m_Ingredient1Icon;
    public Image m_Ingredient2Icon;
    public TextMeshProUGUI m_Ingredient1Amount;
    public TextMeshProUGUI m_Ingredient2Amount;

    private CraftingData m_CurrentRecipe;

    ////////////////
    private void OnEnable()
    {
        CraftRecipeItem.OnClickRecipe += UpdateCurrentRecipe;
    }

    ////////////////
    private void OnDisable()
    {
        CraftRecipeItem.OnClickRecipe -= UpdateCurrentRecipe;
    }

    ////////////////
    private void Start()
    {
        foreach (CraftingData recipe in GameDataStorage.Instance.CraftReceipies)
        {
            CraftRecipeItem item = Instantiate(m_RecipePrefab, m_RecipesRect);
            item.Init(recipe);
        }
    }

    ////////////////
    private void UpdateCurrentRecipe(CraftingData data)
    {
        if (data == null)
            return;

        m_CurrentRecipe = data;

        if (m_CurrentRecipe.CraftItemType == ItemType.equipment)
        {
            EquipmentItem craftItem = GameDataStorage.Instance.GetEquipmentByName(m_CurrentRecipe.CraftItemName);

            m_ItemResultIcon.overrideSprite = craftItem.GetIcon();
        }
        else
        {
            MaterialData craftItem = GameDataStorage.Instance.GetMaterialByName(m_CurrentRecipe.CraftItemName);

            m_ItemResultIcon.overrideSprite = craftItem.GetIcon();
        }
        
        MaterialData ingredient1 = GameDataStorage.Instance.GetMaterialByName(m_CurrentRecipe.Ingredient1);
        MaterialData ingredient2 = GameDataStorage.Instance.GetMaterialByName(m_CurrentRecipe.Ingredient2);

        if (ingredient2 != null)
        {
            m_Ingredient2Icon.gameObject.SetActive(true);
            m_Ingredient2Amount.gameObject.SetActive(true);

            m_Ingredient2Icon.overrideSprite = ingredient2.GetIcon();
            
            string ingredient2text = m_CurrentRecipe.Ingredient2Amount.ToString() + " / "
                + InventoryContent.Instance.GetMaterialAmount(ingredient2);
            m_Ingredient2Amount.text = ingredient2text;
        }
        else
        {
            m_Ingredient2Icon.gameObject.SetActive(false);
            m_Ingredient2Amount.gameObject.SetActive(false);
        }

        m_Ingredient1Icon.overrideSprite = ingredient1.GetIcon();

        string ingredient1text = m_CurrentRecipe.Ingredient1Amount.ToString() + " / "
            + InventoryContent.Instance.GetMaterialAmount(ingredient1);

        m_Ingredient1Amount.text = ingredient1text;
    }

    ////////////////
    public void OnClickCraft()
    {
        if (m_CurrentRecipe == null)
            return;

        MaterialData ingredient1 = GameDataStorage.Instance.GetMaterialByName(m_CurrentRecipe.Ingredient1);
        MaterialData ingredient2 = GameDataStorage.Instance.GetMaterialByName(m_CurrentRecipe.Ingredient2);

        int ing1amount = InventoryContent.Instance.GetMaterialAmount(ingredient1);

        if (ing1amount < m_CurrentRecipe.Ingredient1Amount)
            return;

        int ing2amount = InventoryContent.Instance.GetMaterialAmount(ingredient2);

        if (ing2amount < m_CurrentRecipe.Ingredient2Amount)
            return;

        InventoryContent.Instance.TryRemoveMaterial(ingredient1, m_CurrentRecipe.Ingredient1Amount);
        InventoryContent.Instance.TryRemoveMaterial(ingredient2, m_CurrentRecipe.Ingredient2Amount);

        if (m_CurrentRecipe.CraftItemType == ItemType.equipment)
            InventoryContent.Instance.AddEquipmentItem(m_CurrentRecipe.CraftItemName);
        else
            InventoryContent.Instance.AddMaterial(m_CurrentRecipe.CraftItemName);
    }

    ////////////////
    public void ToggleDialog()
    {
        if (gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
}
