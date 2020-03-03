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
    public TextMeshProUGUI m_ItemInfoText;

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
        IItem craftItem = null;

        // определяем тип предмета
        if (m_CurrentRecipe.CraftItemType == ItemType.equipment)
        {
            craftItem = GameDataStorage.Instance.GetEquipmentByName(m_CurrentRecipe.CraftItemName);
        }
        else
        {
            MaterialData matData = GameDataStorage.Instance.GetMaterialByName(m_CurrentRecipe.CraftItemName);

            craftItem = new MaterialInfo(matData, 0);
        }

        // Отображаем иконку создаваевомого предмета
        m_ItemResultIcon.overrideSprite = craftItem.GetIcon();

        // отображаем ингредиенты для крафта
        UpdateIngredientsView();

        // показываем информацию о предмете крафта
        m_ItemInfoText.text = UIHelper.GetItemInformationText(craftItem);
    }

    ////////////////
    public void OnClickCraft()
    {
        if (m_CurrentRecipe == null)
            return;

        MaterialData ingredient1 = GameDataStorage.Instance.GetMaterialByName(m_CurrentRecipe.Ingredient1);
        MaterialData ingredient2 = GameDataStorage.Instance.GetMaterialByName(m_CurrentRecipe.Ingredient2);

        // проверяем наличие ингредиентов у игрока
        int ing1amount = InventoryContent.Instance.GetMaterialAmount(ingredient1);

        if (ing1amount < m_CurrentRecipe.Ingredient1Amount)
            return;

        int ing2amount = InventoryContent.Instance.GetMaterialAmount(ingredient2);

        if (ing2amount < m_CurrentRecipe.Ingredient2Amount)
            return;

        // тратим ингредиенты
        InventoryContent.Instance.TryRemoveMaterial(ingredient1, m_CurrentRecipe.Ingredient1Amount);
        InventoryContent.Instance.TryRemoveMaterial(ingredient2, m_CurrentRecipe.Ingredient2Amount);

        // добавляем изготовленный предмет
        if (m_CurrentRecipe.CraftItemType == ItemType.equipment)
            InventoryContent.Instance.AddEquipmentItem(m_CurrentRecipe.CraftItemName);
        else
            InventoryContent.Instance.AddMaterial(m_CurrentRecipe.CraftItemName);

        // обновляем отображение
        UpdateIngredientsView();
    }

    ////////////////
    private void UpdateIngredientsView()
    {
        MaterialData ingredient1 = GameDataStorage.Instance.GetMaterialByName(m_CurrentRecipe.Ingredient1);
        MaterialData ingredient2 = GameDataStorage.Instance.GetMaterialByName(m_CurrentRecipe.Ingredient2);

        // если у рецепта 2 ингредиента, то показываем вторую ячейку
        if (ingredient2 != null)
        {
            m_Ingredient2Icon.gameObject.SetActive(true);
            m_Ingredient2Amount.gameObject.SetActive(true);

            m_Ingredient2Icon.overrideSprite = ingredient2.GetIcon();

            string ingredient2text = string.Format("{0} / {1}", InventoryContent.Instance.GetMaterialAmount(ingredient2), m_CurrentRecipe.Ingredient2Amount);
            m_Ingredient2Amount.text = ingredient2text;
        }
        else
        {
            m_Ingredient2Icon.gameObject.SetActive(false);
            m_Ingredient2Amount.gameObject.SetActive(false);
        }

        m_Ingredient1Icon.overrideSprite = ingredient1.GetIcon();

        string ingredient1text = string.Format("{0} / {1}", InventoryContent.Instance.GetMaterialAmount(ingredient1), m_CurrentRecipe.Ingredient1Amount);

        m_Ingredient1Amount.text = ingredient1text;
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
