using SimpleJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager
{
    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new GameManager();

            return m_Instance;
        }
    }

    /////////////////
    public void StartGame(JsonObject json)
    {
        Debug.Log(json);

        if (json != null)
        {
            JsonObject inventoryContent = json.Get<JsonObject>("inventory");
            InventoryContent.Instance.Init(inventoryContent);

            JsonObject profile = json.Get<JsonObject>("profile");
            PlayerProfile.Instance.LoadProfile(profile);
        }
        else
        {
            PlayerProfile.Instance.CreateNewProfile(GameDataStorage.Instance.NewProfileData);
        }
        
        SceneManager.LoadScene(SceneName.Home);
        
        PlayerProfile.OnEquipmentChanged += SaveGame;
    }

    /////////////////
    public void SaveGame()
    {
        // сохраняем данные инвентаря
        JsonObject dataToSave = new JsonObject();

        JsonObject inventoryContent = new JsonObject();

        // сохраняем экипировку
        List<EquipmentItem> equipments = InventoryContent.Instance.PlayerEquipments;

        JsonArray equipmentsArray = new JsonArray();
        
        foreach (EquipmentItem item in equipments)
        {
            equipmentsArray.Add(item.Name);
        }

        inventoryContent.Add("equipments", equipmentsArray);
        
        // сохраняем материалы
        List<MaterialInfo> materials = InventoryContent.Instance.PlayerMaterials;

        JsonArray materialsArray = new JsonArray();
        
        foreach (MaterialInfo item in materials)
        {
            JsonObject materialJson = new JsonObject()
            {
                { "name", item.Data.Name },
                { "amount", item.Amount }
            };

            materialsArray.Add(materialJson);
        }

        inventoryContent.Add("materials", materialsArray);

        dataToSave.Add("inventory", inventoryContent);

        // сохраняем данные персонажа и прогресса игрока
        dataToSave.Add("profile", PlayerProfile.Instance.SaveProfile());

        // после формирования файла сохраняем
        PlayerPrefs.SetString(Constants.SavedGame, dataToSave.ToString());
        PlayerPrefs.Save();

        Debug.Log("Game Saved!");
    }
}
