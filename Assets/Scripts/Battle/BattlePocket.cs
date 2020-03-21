using UnityEngine;
using UnityEngine.UI;

public class BattlePocket : MonoBehaviour
{
    public Image m_Icon;

    private MaterialInfo m_Item;
    private PlayerHero m_Hero;

    /////////////
    public void SetupPocket(MaterialInfo item, PlayerHero hero)
    {
        if (item == null || hero == null)
            return;

        m_Item = item;
        m_Hero = hero;

        m_Icon.overrideSprite = m_Item.Data.GetIcon();
    }

    /////////////
    public void OnClickPocket()
    {
        if (m_Item == null || m_Hero == null)
            return;

        // TODO разделить логику для вредящих и помогающих предметов
        // if (вредящее)
        // берем текущий таргет героя, если он есть
        // else (хилящее, бафающее)
        // применяем к герою
        m_Item.ApplyConsumeEffect(m_Hero);

        m_Item = null;
        m_Icon.overrideSprite = null;
    }
}
