using SimpleJson;

public class CraftingData
{
    public string m_CraftItem { get; private set; }
    public string m_Ingredient1 { get; private set; }
    public int m_Ingredient1Amount { get; private set; }
    public string m_Ingredient2 { get; private set; }
    public int m_Ingredient2Amount { get; private set; }

    ////////////////
    public CraftingData(JsonObject json)
    {
        m_CraftItem = (string)json["name"];
        m_Ingredient1 = (string)json["ingredient_1"];
        m_Ingredient2 = (string)json["ingredient_2"];
        m_Ingredient1Amount = json.GetInt("ingredient_1_amount");
        m_Ingredient2Amount = json.GetInt("ingredient_2_amount");
    }
}
