using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class CKTools
{
    [MenuItem("Tools/Open Scene/MainMenu")]
    public static void OpenMainMenuScene()
    {
        OpenScene(SceneName.MainMenu);
    }

    [MenuItem("Tools/Open Scene/Home")]
    public static void OpenHomeScene()
    {
        OpenScene(SceneName.Home);
    }

    [MenuItem("Tools/Open Scene/Battle")]
    public static void OpenBattleScene()
    {
        OpenScene(SceneName.BattleScene);
    }

    ///////////////
    private static void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");
    }
}
