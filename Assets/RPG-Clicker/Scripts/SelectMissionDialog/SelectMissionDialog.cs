using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMissionDialog : BaseDialog
{
    [SerializeField] private MissionItem m_MissionPrefab;
    [SerializeField] private RectTransform m_MissionsContainer;

    private bool m_IsInited = false;

    //////////////////
    public override void Show()
    {
        base.Show();

        InitView();
    }

    //////////////////
    private void InitView()
    {
        if (m_IsInited)
            return;

        List<MissionData> missions = GameDataStorage.Instance.Missions;

        missions.Sort((a, b) =>
        {
            if (a.Number > b.Number)
                return 1;
            else
                return -1;
        });

        foreach (MissionData mission in missions)
        {
            if (mission.Number > PlayerProfile.Instance.NormalWorldMissionNumber) // здесь указываем максимальный уровень миссии в зависимости от мира, в который идем
                continue;

            MissionItem missionItem = Instantiate(m_MissionPrefab, m_MissionsContainer);
            missionItem.Setinfo(mission);
        }

        m_IsInited = true;
    }
}
