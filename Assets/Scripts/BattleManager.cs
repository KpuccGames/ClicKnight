using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private PlayerHero m_PlayerHero;

    //////////////
    private void Start()
    {
        m_PlayerHero = GameObject.Find("PlayerHero").GetComponent<PlayerHero>();
    }

    //////////////
    public void OnClick()
    {
        // TODO реализация атаки
        m_PlayerHero.Attack();
    }
}
