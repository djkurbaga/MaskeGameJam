using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class InventoryItem
{
    public string itemName; // Item ID
    public Sprite icon;     // UI'da görünecek resim
    [TextArea] public string description; // Opt.
}

public class InventorySlot : MonoBehaviour
{
    public int slotIndex;

    [Header("UI References")]
    public Image iconImage;      
    public Button slotButton;   
    public GameObject highlight; 

    private InventoryItem currentItem; 

    public void Initialize(int index, Action<int> onClickAction)
    {
        slotIndex = index;
        if (slotButton != null)
        {
            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(() => onClickAction(index));
        }

        ClearSlot(); 
    }

    public void SetItem(InventoryItem newItem)
    {
        currentItem = newItem;
        iconImage.sprite = newItem.icon;
        iconImage.enabled = true; 
        iconImage.preserveAspect = true; 
    }

    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.enabled = false; 
        SetHighlight(false);
    }

    public void SetHighlight(bool active)
    {
        if (highlight != null) highlight.SetActive(active);
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public InventoryItem GetItem()
    {
        return currentItem;
    }
}