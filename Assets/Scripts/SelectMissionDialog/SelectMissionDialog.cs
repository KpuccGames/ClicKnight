using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMissionDialog : BaseDialog
{
    [SerializeField] private MissionItem m_MissionPrefab;
    [SerializeField] private RectTransform m_MissionsContainer;

    //////////////////
    public override void Show()
    {
        base.Show();

        InitView();
    }

    //////////////////
    private void InitView()
    {
        List<MissionData> missions = GameDataStorage.Instance.Missions;

        foreach (MissionData mission in missions)
        {
            MissionItem missionItem = Instantiate(m_MissionPrefab, m_MissionsContainer);
            missionItem.Setinfo(mission);
        }
    }
}
