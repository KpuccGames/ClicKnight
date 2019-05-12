using SimpleJson;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        InventoryContent.Instance.Init(json);

        SceneManager.LoadScene(SceneName.Home);
    }
}
