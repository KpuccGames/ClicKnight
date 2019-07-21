using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDialog : MonoBehaviour
{
    //////////////////
    public virtual void Show()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
    }

    //////////////////
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
