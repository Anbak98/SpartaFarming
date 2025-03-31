using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 슬롯 UI 컴포넌트
/// </summary>
public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI 요소")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _quantityText;
    
    [Header("슬롯 설정")]
    [SerializeField] private int _slotIndex;
    
    // 참조
    private StorageUI _storageUI;
    
    // 속성
    public int SlotIndex 
    { 
        get => _slotIndex;
        set => _slotIndex = value;
    }
    
    /// <summary>
    /// 초기화
    /// </summary>
    private void Awake()
    {
        _storageUI = GetComponentInParent<StorageUI>();
        DisableUI();
    }
    
    /// <summary>
    /// 아이템 UI 업데이트
    /// </summary>
    /// <param name="item">표시할 아이템</param>
    public void UpdateUI(ItemInstance item)
    {
        if (item == null)
        {
            DisableUI();
            return;
        }
        
        EnableUI();
        
        // 아이템 아이콘 설정
        _icon.sprite = Resources.Load<Sprite>(item.ItemInfo.spritePath);
        _icon.SetNativeSize();
        
        // 수량 텍스트 설정 (1개일 경우 표시 안함)
        _quantityText.text = item.Quantity > 1 ? item.Quantity.ToString() : string.Empty;
    }
    
    /// <summary>
    /// UI 비활성화
    /// </summary>
    public void DisableUI()
    {
        _icon.enabled = false;
        _quantityText.enabled = false;
    }
    
    /// <summary>
    /// UI 활성화
    /// </summary>
    public void EnableUI()
    {
        _icon.enabled = true;
        _quantityText.enabled = true;
    }
    
    /// <summary>
    /// 포인터 클릭 이벤트 처리
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_storageUI == null) return;
        
        // 좌클릭 처리
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _storageUI.OnSlotLeftClicked(_slotIndex);
        }
        // 우클릭 처리
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            _storageUI.OnSlotRightClicked(_slotIndex);
        }
    }
}
