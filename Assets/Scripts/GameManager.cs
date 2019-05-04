using SimpleJson;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void StartGame(JsonObject json)
    {
        DontDestroyOnLoad(this);

        InventoryContent.Instance.Init(json);

        SceneManager.LoadScene(SceneName.Home);
    }
}
