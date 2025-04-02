using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public PlayerMenuUI playerMenuUI;
    public ShopUI shopUI;

    private void Awake()
    {
        playerMenuUI = FindObjectOfType<PlayerMenuUI>();
        shopUI = FindObjectOfType<ShopUI>();
    }

    public void OpenPlayerMenuUI()
    {
        playerMenuUI.gameObject.SetActive(true);
    }

    public void ClosePlayerMenuUI()
    {
        playerMenuUI.gameObject.SetActive(false);
    }

    public void OpenShopUI(Shop shop)
    {
        shopUI.Init(shop);
        shopUI.gameObject.SetActive(true);
    }

    public void CloseShopUI()
    {
        shopUI.gameObject.SetActive(false);
    }
    
}
