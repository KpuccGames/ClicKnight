using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    public Image m_Icon;

    //////////////
    public void SetItemIcon(Sprite icon)
    {
        m_Icon.overrideSprite = icon;
    }
}
