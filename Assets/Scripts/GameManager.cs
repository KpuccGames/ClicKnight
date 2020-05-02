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

    private readonly IDataStorage[] m_Storages = new IDataStorage[]
    {
        MaterialsDataStorage.Instance,
        EquipmentsDataStorage.Instance,
        DropsDataStorage.Instance,
        EnemiesDataStorage.Instance,
        MissionsDataStorage.Instance,
        AbilitiesDataStorage.Instance,
        CraftingDataStorage.Instance
    };

    private JsonArray m_NewProfileData;

    public void InitData()
    {
        GameDataContainer gameDatas = Resources.Load<GameDataContainer>("GameDataContainer");

        for (int i = 0; i < m_Storages.Length; i++)
        {
            string storageName = m_Storages[i].GetStorageName();

            foreach (TextAsset asset in gameDatas.m_GameDataFiles)
            {
                if (asset.name.Equals(storageName))
                {
                    m_Storages[i].Init(Helper.ParseJsonArray(asset.ToString()));
                }
            }
        }

        m_NewProfileData = Helper.ParseJsonArray(gameDatas.m_BaseConfig.ToString());
    }

    /////////////////
    public void StartGame(JsonObject json)
    {
        Debug.Log(json);

        if (json != null)
        {
            JsonObject inventoryContent = json.Get<JsonObject>("inventory");
            Inventory.Instance.Init(inventoryContent);

            JsonObject profile = json.Get<JsonObject>("profile");
            PlayerProfile.Instance.LoadProfile(profile, m_NewProfileData);
        }
        else
        {
            PlayerProfile.Instance.CreateNewProfile(m_NewProfileData);
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
        List<EquipmentInfo> equipments = Inventory.Instance.PlayerEquipments;

        JsonArray equipmentsArray = new JsonArray();
        
        foreach (EquipmentInfo item in equipments)
        {
            equipmentsArray.Add(item.Data.Name);
        }

        inventoryContent.Add("equipments", equipmentsArray);
        
        // сохраняем материалы
        List<MaterialInfo> materials = Inventory.Instance.PlayerMaterials;

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
