using SimpleJson;

public class CraftingData
{
    public string CraftItemName { get; private set; }
    public string Ingredient1 { get; private set; }
    public int Ingredient1Amount { get; private set; }
    public string Ingredient2 { get; private set; }
    public int Ingredient2Amount { get; private set; }
    public ItemType CraftItemType { get; private set; }

    ////////////////
    public CraftingData(JsonObject json)
    {
        CraftItemName = (string)json["name"];
        
        //
        //TODO ингредиенты сделать списком
        //

        Ingredient1 = (string)json["ingredient_1"];
        Ingredient2 = json.GetString("ingredient_2", string.Empty);

        if (Ingredient1.Equals(Ingredient2))
            UnityEngine.Debug.LogError("Duplicate ingredients in recipe: " + CraftItemName);

        Ingredient1Amount = json.GetInt("ingredient_1_amount");
        Ingredient2Amount = json.GetInt("ingredient_2_amount", 0);
        CraftItemType = (ItemType)json.GetInt("item_type");
    }
}
