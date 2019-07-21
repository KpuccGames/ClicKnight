using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_MissionNumberText;

    private MissionData m_MissionData;

    //////////////////
    public void Setinfo(MissionData data)
    {
        m_MissionData = data;
        m_MissionNumberText.text = data.Number.ToString();
    }

    //////////////////
    public void OnClick()
    {
        BattleManager.StartMission(m_MissionData);
    }
}
