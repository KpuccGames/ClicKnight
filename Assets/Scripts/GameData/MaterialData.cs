using SimpleJson;

public class MaterialData
{
    public string Name { get; private set; }
    private string m_Icon;

    ////////////////
    public MaterialData(JsonObject json)
    {
        Name = (string)json["name"];
        m_Icon = (string)json["icon"];
    }
}
