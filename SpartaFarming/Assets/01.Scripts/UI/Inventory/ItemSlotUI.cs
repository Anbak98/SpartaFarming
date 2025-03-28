using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    private StorageUI storageUI;

    public Image icon;
    public TextMeshProUGUI quantityText;
    public int slotIndex;

    private void Awake()
    {
        storageUI = GetComponentInParent<StorageUI>();
    }

    public void UpdateUI(ItemInstance item)
    {
        OnUI();
        icon.sprite = Resources.Load<Sprite>(item.itemInfo.spritePath);
        icon.SetNativeSize();
        quantityText.text = item.quantity.ToString();
    }

    public void OffUI()
    {
        icon.enabled = false;
        quantityText.enabled = false;
    }

    public void OnUI()
    {
        icon.enabled = true;
        quantityText.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        storageUI.OnClickSlotUI(slotIndex);
    }
}
