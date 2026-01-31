using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI References")]
    public GameObject inventoryPanel; // Açýlýp kapanacak ana panel
    public Transform slotContainer;   // Grid Layout'un olduðu obje (Slotlarýn babasý)

    [Header("Data")]
    public List<InventoryItem> itemPool; // Oyundaki tüm eþyalarýn listesi (Database)
    public List<InventorySlot> slots = new List<InventorySlot>();

    private InventorySlot selectedSlot; // Þu an seçili olan slot

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Baþlangýçta paneli gizle
        inventoryPanel.SetActive(false);
    }

    private void Start()
    {
        // Slotlarý otomatik bul ve ayarla
        SetupSlots();
    }

    // Grid altýndaki slotlarý listeye çeker ve indexlerini verir
    void SetupSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] != null)
            {
                // Ýndeksi ve týklama fonksiyonunu manuel olarak baðlýyoruz
                slots[i].Initialize(i, OnSlotClicked);
            }
        }
    }

    // --- TEMEL FONKSÝYONLAR ---

    // GameManager (I tuþu) burayý çaðýracak
    public void ToggleInventory()
    {
        IsOpen = !IsOpen;
        inventoryPanel.SetActive(IsOpen);

        if (IsOpen)
        {
            GameManager.Instance.CurrentState = GameState.Inventory;
            DeselectAll();
        }
        else
        {
            GameManager.Instance.CurrentState = GameState.Gameplay;
            if (InteractionManager.Instance) InteractionManager.Instance.SetInteractionState(true);
        }
    }

    // itemPool'daki index'e veya isme göre eþya ekle
    public void AddItem(string itemName)
    {
        // 1. Ýsimden itemi bul
        InventoryItem itemToAdd = itemPool.Find(x => x.itemName == itemName);

        if (itemToAdd == null)
        {
            Debug.LogWarning("Inventory: Item bulunamadý -> " + itemName);
            return;
        }

        // 2. Ýlk boþ slotu bul ve yerleþtir
        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(itemToAdd);
                Debug.Log(itemName + " envantere eklendi.");
                return;
            }
        }

        Debug.Log("Envanter DOLU!");
    }

    // Slotlardan birine týklandýðýnda çalýþýr
    public void OnSlotClicked(int index)
    {
        DeselectAll();

        selectedSlot = slots[index];
        selectedSlot.SetHighlight(true);

        if (!selectedSlot.IsEmpty())
        {
            Debug.Log("Seçilen Eþya: " + selectedSlot.GetItem().itemName);
            // TODO: Eþya kullanma iþlemi.
        }
    }

    // Seçili eþyayý kullanma (ActionManager veya GameManager çaðýrabilir)
    public void UseSelectedItem()
    {
        if (selectedSlot != null && !selectedSlot.IsEmpty())
        {
            string itemName = selectedSlot.GetItem().itemName;
            Debug.Log(itemName + " kullanýldý.");

            // Eþyayý kullandýk, siliyoruz (Tüketilen eþya ise)
            selectedSlot.ClearSlot();
            selectedSlot = null;
        }
    }

    // Belirli bir eþyaya sahip miyiz? (Mektup okuma kontrolü vb.)
    public bool HasItem(string itemName)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty() && slot.GetItem().itemName == itemName)
                return true;
        }
        return false;
    }

    void DeselectAll()
    {
        if (selectedSlot != null) selectedSlot.SetHighlight(false);
        selectedSlot = null;
    }
}