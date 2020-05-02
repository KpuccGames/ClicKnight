using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GameDataContainer")]
public class GameDataContainer : ScriptableObject
{
    public TextAsset[] m_GameDataFiles;
    public TextAsset m_BaseConfig;
}
