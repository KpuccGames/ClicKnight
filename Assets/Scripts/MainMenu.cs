using SimpleJson;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button m_NewGameButton;
    public Button m_ContinueGameButton;

    ////////////////
    private void Start()
    {
        GameDataStorage.Instance.Init();
    }

    ////////////////
    private void SetupMainMenuView()
    {
        m_NewGameButton.gameObject.SetActive(true);
        m_ContinueGameButton.interactable = CheckSavedGame();
    }

    ////////////////
    private bool CheckSavedGame()
    {
        return PlayerPrefs.HasKey(Constants.SavedGame);
    }

    ////////////////
    public void OnNewGameClick()
    {
        if (CheckSavedGame())
        {
            // если есть сохраненная игра, топ редупреждать пользователя, что она затрется после начала новой
            // return;
        }

        PlayerPrefs.SetString(Constants.SavedGame, string.Empty);
        PlayerPrefs.Save();

        LoadGame(null);
    }

    ////////////////
    public void OnContinueGameClick()
    {
        if (!CheckSavedGame())
            return;

        string savedData = PlayerPrefs.GetString(Constants.SavedGame);

        if (string.IsNullOrEmpty(savedData))
            return;

        JsonObject json = Helper.ParseJson(savedData);

        LoadGame(json);
    }

    ////////////////
    private void LoadGame(JsonObject json)
    {
        GameManager.Instance.StartGame(json);
    }
}
