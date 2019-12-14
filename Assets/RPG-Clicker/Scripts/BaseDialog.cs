using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDialog : MonoBehaviour
{
    //////////////////
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    //////////////////
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
