using UnityEngine.UI;
using UnityEngine;
using System;

public class InventoryPocketItem : MonoBehaviour
{

    public Image m_PocketItemIcon;
    public Button m_RemoveFromPocketButton;
    
    private MaterialInfo m_PocketItem;
    private int m_PocketNumber;

    public static event Action<InventoryPocketItem> OnPocketItemClicked;

    private void OnEnable()
    {
        OnPocketItemClicked += OnItemClicked;
    }

    private void OnDisable()
    {
        OnPocketItemClicked -= OnItemClicked;
    }

    public void InitItem(MaterialInfo material, int pocketNumber)
    {
        m_PocketNumber = pocketNumber;
        m_RemoveFromPocketButton.gameObject.SetActive(false);

        UpdateItem(material);
    }

    public void UpdateItem(MaterialInfo material)
    {
        if (material == null)
        {
            m_PocketItem = null;
            m_PocketItemIcon.overrideSprite = null;
            return;
        }

        if (!material.IsConsumable())
            return;

        m_PocketItem = material;
        m_PocketItemIcon.overrideSprite = material.Data.GetIcon();
    }

    public void RemoveItemFromPocket()
    {
        PlayerProfile.Instance.RemoveItemFromPocket(m_PocketNumber, false);
        m_RemoveFromPocketButton.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        OnPocketItemClicked?.Invoke(this);
    }

    private void OnItemClicked(InventoryPocketItem item)
    {
        bool needShowItemButton = item.m_PocketNumber == m_PocketNumber && m_PocketItem != null;

        m_RemoveFromPocketButton.gameObject.SetActive(needShowItemButton);
    }
}
