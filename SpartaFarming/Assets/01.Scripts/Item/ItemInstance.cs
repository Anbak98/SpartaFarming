using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance
{
    public ItemInfo ItemInfo { get; }

    public float currentDurability;
    public bool isHolding;
    public bool isEquiped;

    public ItemInstance(ItemInfo itemInfo)
    {
        ItemInfo = itemInfo;
        currentDurability = itemInfo.durability;
    }
}
