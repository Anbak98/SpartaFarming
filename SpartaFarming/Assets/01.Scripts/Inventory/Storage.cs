using System;
using System.Collections.Generic;
using System.Diagnostics;

[Serializable]
public class Storage
{
    public int maxCount { get; private set; }
    public List<ItemInstance> items;

    public event Action<Storage> onChangeStorage;

    public Storage(int maxCount)
    {
        this.maxCount = maxCount;
        InitializeItems();
    }

    private void InitializeItems()
    {
        items = new List<ItemInstance>();
        for (int i = 0; i < maxCount; i++)
        {
            items.Add(null);
        }
    }

    public bool AddItem(ItemInstance item)
    {
        if (TryStackExistingItems(item))
            return true;

        return PlaceInEmptySlots(item);
    }

    private bool TryStackExistingItems(ItemInstance item)
    {
        if (item.itemInfo.maxStack <= 1)
            return false;

        for (int i = 0; i < maxCount; i++)
        {
            if (CanStackWithSlot(i, item))
            {
                if (StackWithSlot(i, item))
                    return true;
            }
        }

        return false;
    }

    private bool CanStackWithSlot(int slotIndex, ItemInstance item)
    {
        if (items[slotIndex] == null)
            return false;

        if (items[slotIndex].quantity >= items[slotIndex].itemInfo.maxStack)
            return false;

        return items[slotIndex].itemInfo.key == item.itemInfo.key;
    }

    private bool StackWithSlot(int slotIndex, ItemInstance item)
    {
        int total = items[slotIndex].quantity + item.quantity;
        int max = item.itemInfo.maxStack;
        
        if (total <= max)
        {
            items[slotIndex].quantity = total;
            item.quantity = 0;
            NotifyStorageChanged();
            return true;
        }
        else
        {
            items[slotIndex].quantity = max;
            item.quantity = total - max;
            return false;
        }
    }

    private bool PlaceInEmptySlots(ItemInstance item)
    {
        List<int> emptySlots = FindEmptySlots();
        
        for (int i = 0; i < emptySlots.Count && item.quantity > 0; i++)
        {
            int index = emptySlots[i];

            if (item.quantity <= item.itemInfo.maxStack)
            {
                items[index] = item;
                NotifyStorageChanged();
                return true;
            }
            else
            {
                ItemInstance newItem = new ItemInstance(item.itemInfo, item.itemInfo.maxStack);
                items[index] = newItem;
                item.quantity -= item.itemInfo.maxStack;
            }
        }

        NotifyStorageChanged();
        return false;
    }

    private List<int> FindEmptySlots()
    {
        List<int> emptySlots = new List<int>();
        for (int i = 0; i < maxCount; i++)
        {
            if (items[i] == null)
            {
                emptySlots.Add(i);
            }
        }
        return emptySlots;
    }

    public bool TryAddItemAt(int index, ItemInstance item)
    {
        if (items[index] == null)
        {
            items[index] = item;
            NotifyStorageChanged();
            return true;
        }

        if (items[index].itemInfo.maxStack > 1 && items[index].itemInfo.key == item.itemInfo.key)
        {
            return TryStackAtSlot(index, item);
        }

        NotifyStorageChanged();
        return false;
    }

    private bool TryStackAtSlot(int index, ItemInstance item)
    {
        int total = items[index].quantity + item.quantity;
        int max = items[index].itemInfo.maxStack;

        if (total <= max)
        {
            items[index].quantity = total;
            NotifyStorageChanged();
            return true;
        }
        else
        {
            items[index].quantity = max;
            item.quantity = total - max;
            NotifyStorageChanged();
            return false;
        }
    }

    public void RemoveItem(int key, ref int quantity)
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (items[i] == null)
                continue;

            if (items[i].itemInfo.key == key)
            {
                if (RemoveQuantityFromSlot(i, ref quantity))
                    return;
            }
        }

        NotifyStorageChanged();
    }

    private bool RemoveQuantityFromSlot(int slotIndex, ref int quantity)
    {
        int total = items[slotIndex].quantity - quantity;
        if (total <= 0)
        {
            quantity -= items[slotIndex].quantity;
            items[slotIndex] = null;
            return false;
        }
        else
        {
            quantity = 0;
            items[slotIndex].quantity = total;
            NotifyStorageChanged();
            return true;
        }
    }

    public void RemoveItemAt(int index)
    {
        items[index] = null;
        NotifyStorageChanged();
    }

    public void SwapItem(int index, ref ItemInstance item)
    {
        ItemInstance temp = items[index];
        items[index] = item;
        item = temp;

        NotifyStorageChanged();
    }

    private void NotifyStorageChanged()
    {
        onChangeStorage?.Invoke(this);
    }
}
