using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    private Shop _shop;
    private List<ShopItemSlotUI> _itemSlots = new List<ShopItemSlotUI>();
    [SerializeField] private ShopItemSlotUI shopItemSlotPrefab;
    [SerializeField] private Transform shopItemSlotParent;
    [SerializeField] private HoldItemUI holdItemSlotUI;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public void Init(Shop shop)
    {
        _shop = shop;
        UpdateShopItems();
    }

    public void UpdateShopItems()
    {
        DestoryShopItemSlots();
        CreateShopItemSlots();
    }

    public void CreateShopItemSlots()
    {
        for(int i = 0; i < _shop.ShopItems.Count; i++)
        {
            ShopItemSlotUI shopItemSlot = Instantiate(shopItemSlotPrefab, shopItemSlotParent);
            shopItemSlot._slotIndex = i;
            shopItemSlot.UpdateUI(_shop.ShopItems[i]);
            _itemSlots.Add(shopItemSlot);
        }
    }

    public void DestoryShopItemSlots()
    {
        foreach (var item in _itemSlots)
        {
            Destroy(item.gameObject);
        }

        _itemSlots.Clear();
    }

    public void BuyItem(int index)
    {
        _shop.BuyItem(player, index);
    }
}
