using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoldItemUI : MonoBehaviour
{
    [SerializeField] private Image holdItemImage;
    [SerializeField] private TextMeshProUGUI holdItemText;

    public void UpdateHoldItem(ItemInstance item)
    {
        holdItemImage.sprite = Resources.Load<Sprite>(item.itemInfo.spritePath);
        holdItemImage.SetNativeSize();

        holdItemText.text = item.quantity.ToString();
    }
}
