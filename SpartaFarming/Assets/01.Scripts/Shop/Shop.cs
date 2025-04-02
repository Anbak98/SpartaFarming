using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [field:SerializeField] public List<int> ShopItems { get; private set; }

    public void Start()
    {
        UIManager.Instance.OpenShopUI(this);
    }

    public void BuyItem(Player player,int index)
    {
        if(index >= ShopItems.Count) return;

        ItemInfo itemInfo = DataManager.ItemLoader.GetByKey(ShopItems[index]);
        ItemInstance itemInstance = new ItemInstance(itemInfo);
        player.Controller.Inventory.AddItem(itemInstance);
    }
}
