using System;
using System.Collections.Generic;

[Serializable]
public class Storage
{
    public int maxCount { get; private set; }
    public List<ItemInstance> items;

    public Storage(int maxCount)
    {
        this.maxCount = maxCount;
    }

    public bool AddItem(ItemInstance item)
    {
        ItemInfo itemInfo = item.itemInfo;

        if (itemInfo.maxStack > 1)
        {
            for(int i = 0; i  < items.Count; i++)
            {
                if (items[i].quantity >= items[i].itemInfo.maxStack)
                    continue;

                if (items[i].itemInfo.key == itemInfo.key)
                {
                    int total = items[i].quantity + item.quantity;
                    int max = itemInfo.maxStack;
                    if (total <= max)
                    {
                        items[i].quantity = total;
                        item.quantity = 0;
                        return true;
                    }
                    else
                    {
                        items[i].quantity = max;
                        item.quantity = total - max;
                    }
                }
            }
        }

        while(item.quantity > 0)
        {
            if (items.Count >= maxCount) break;

            if(item.quantity <= itemInfo.maxStack)
            {
                items.Add(item);
                return true;
            }
            else
            {
                item.quantity -= itemInfo.maxStack;
                ItemInstance newItem = new ItemInstance(itemInfo, itemInfo.maxStack);
                items.Add(newItem);
            }
        }

        return false;
    }
}
