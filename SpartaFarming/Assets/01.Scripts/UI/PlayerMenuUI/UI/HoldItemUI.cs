using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 마우스로 들고 있는 아이템을 표시하는 UI 컴포넌트
/// </summary>
public class HoldItemUI : MonoBehaviour
{
    [Header("UI 요소")]
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _quantityText;
    
    /// <summary>
    /// 마우스 커서에 표시될 아이템 정보 업데이트
    /// </summary>
    /// <param name="item">표시할 아이템</param>
    public void UpdateHoldItem(ItemInstance item)
    {
        if (item == null)
        {
            gameObject.SetActive(false);
            return;
        }
        
        // 아이템 이미지 업데이트
        if (_itemImage != null)
        {
            _itemImage.sprite = Resources.Load<Sprite>(item.ItemInfo.spritePath);
            _itemImage.SetNativeSize();
        }
        
        // 수량 텍스트 업데이트 (1개일 경우 표시 안함)
        if (_quantityText != null)
        {
            _quantityText.text = item.Quantity > 1 ? item.Quantity.ToString() : string.Empty;
        }
    }
    
    /// <summary>
    /// 들고 있는 아이템 UI 초기화
    /// </summary>
    public void Clear()
    {
        gameObject.SetActive(false);
    }
}
