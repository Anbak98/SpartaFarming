using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ShopItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image itemIcon;
    public TextMeshProUGUI itemPriceText;
    public int _slotIndex;

    private ShopUI shopUI;

    private void Awake()
    {
        shopUI = GetComponentInParent<ShopUI>();
    }

    public void UpdateUI(int itemId)
    {
        ItemInfo itemInfo = DataManager.ItemLoader.GetByKey(itemId);
        itemIcon.sprite = Resources.Load<Sprite>(itemInfo.spritePath);
        itemPriceText.text = itemInfo.price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        shopUI.BuyItem(_slotIndex);
    }
}
