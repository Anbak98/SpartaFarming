using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemInstance
{
    public ItemInfo itemInfo { get; }

    public float currentDurability;
    public int quantity;
    public bool isHolding = false;
    public bool isEquiped = false;

    public ItemInstance(ItemInfo itemInfo, int quantity = 1)
    {
        this.itemInfo = itemInfo;
        this.quantity = quantity;
    }
}
