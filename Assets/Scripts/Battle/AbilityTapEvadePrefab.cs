using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityTapEvadePrefab : MonoBehaviour
{
    private Coroutine m_ProcessCoroutine;

    /////////////////
    public void Setup(int damage, float reactionTime)
    {
        m_ProcessCoroutine = StartCoroutine(StartProcessCoroutine(damage, reactionTime));

        Enemy.OnEnemyDeath += OnEnemyDied;
    }

    /////////////////
    private void OnDestroy()
    {
        Enemy.OnEnemyDeath -= OnEnemyDied;
    }

    /////////////////
    private IEnumerator StartProcessCoroutine(int damage, float reactionTime)
    {
        yield return new WaitForSecondsRealtime(reactionTime);

        BattleManager.Instance.m_PlayerHero.TakeDamage(damage);

        Destroy(gameObject);
    }

    /////////////////
    public void OnClick()
    {
        if (m_ProcessCoroutine != null)
        {
            StopCoroutine(m_ProcessCoroutine);
            m_ProcessCoroutine = null;
        }

        Debug.Log("Ability evaded!");

        Destroy(gameObject);
    }

    /////////////////
    private void OnEnemyDied()
    {
        if (m_ProcessCoroutine != null)
        {
            StopCoroutine(m_ProcessCoroutine);
            m_ProcessCoroutine = null;
        }

        Destroy(gameObject);
    }
}
