using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class StorageUI : MonoBehaviour
{
    public List<ItemSlotUI> itemSlots = new List<ItemSlotUI>();
    public StorageType storageType;
    private InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();

        ItemSlotUI[] slots = GetComponentsInChildren<ItemSlotUI>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotIndex = i;
            itemSlots.Add(slots[i]);
        }
    }

    public void UpdateUI(Storage storage)
    {
        for (int i = 0; i < storage.items.Count; i++)
        {
            itemSlots[i].OffUI();

            if (storage.items[i] != null)
                itemSlots[i].UpdateUI(storage.items[i]);
        }
    }

    public void OnClickSlotUI(int slotIndex)
    {
        inventoryUI.OnClickSlotUI(slotIndex, storageType);
    }
}
