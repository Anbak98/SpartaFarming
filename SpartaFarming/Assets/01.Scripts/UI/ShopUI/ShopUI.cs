using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    private Shop _shop;
    private ItemSlotUI[] _itemSlots;

    private void Awake()
    {
        _itemSlots = GetComponentsInChildren<ItemSlotUI>();
    }

    public void Init(Shop shop)
    {
        _shop = shop;
    }

    public void UpdateShopItems()
    {

    }
}
