using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 UI를 총괄 관리하는 컴포넌트
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private StorageUI _mainStorageUI;
    [SerializeField] private StorageUI _quickStorageUI;
    [SerializeField] private HoldItemUI _holdItemUI;
    
    [Header("데이터 참조")]
    [SerializeField] private Inventory _inventory;
    
    // 상태
    private ItemInstance _holdItem;
    private bool _isHoldingItem = false;
    private bool _isSplitMode = false;
    private bool _isInventoryVisible = false;
    
    // 컴포넌트 참조
    private InventoryInputSystem _inputSystem;
    
    // 속성
    public bool IsInventoryVisible => _isInventoryVisible;
    public bool IsHoldingItem => _isHoldingItem;
    
    /// <summary>
    /// 초기화
    /// </summary>
    private void Awake()
    {
        _inputSystem = GetComponent<InventoryInputSystem>();
        _inventory = FindObjectOfType<Inventory>();
    }
    
    /// <summary>
    /// 이벤트 연결
    /// </summary>
    private void Start()
    {
        // 입력 시스템 이벤트 연결
        if (_inputSystem != null)
        {
            _inputSystem.onSplitModeChanged += SetSplitMode;
        }
        
        // 인벤토리 이벤트 연결
        if (_inventory != null)
        {
            // 메인 저장소와 퀵 저장소 UI 업데이트 이벤트 연결
            if (_mainStorageUI != null && _inventory.MainStorage != null)
            {
                _inventory.MainStorage.onChangeStorage += _mainStorageUI.UpdateUI;
                _mainStorageUI.onSlotLeftClicked += HandleMainStorageLeftClick;
                _mainStorageUI.onSlotRightClicked += HandleMainStorageRightClick;
            }
            
            if (_quickStorageUI != null && _inventory.QuickStorage != null)
            {
                _inventory.QuickStorage.onChangeStorage += _quickStorageUI.UpdateUI;
                _quickStorageUI.onSlotLeftClicked += HandleQuickStorageLeftClick;
                _quickStorageUI.onSlotRightClicked += HandleQuickStorageRightClick;
            }
            
            // 초기 UI 업데이트
            if (_mainStorageUI != null && _inventory.MainStorage != null)
            {
                _mainStorageUI.UpdateUI(_inventory.MainStorage);
            }
            
            if (_quickStorageUI != null && _inventory.QuickStorage != null)
            {
                _quickStorageUI.UpdateUI(_inventory.QuickStorage);
            }
        }
    }
    
    /// <summary>
    /// 매 프레임 업데이트
    /// </summary>
    private void Update()
    {
        // 아이템을 들고 있을 때 마우스 커서 위치 업데이트
        if (_isHoldingItem && _holdItemUI != null)
        {
            _holdItemUI.transform.position = Input.mousePosition;
        }
    }
    
    /// <summary>
    /// 분할 모드 설정
    /// </summary>
    /// <param name="isSplitMode">분할 모드 여부</param>
    public void SetSplitMode(bool isSplitMode)
    {
        _isSplitMode = isSplitMode;
    }
    
    #region 메인 저장소 상호작용
    
    /// <summary>
    /// 메인 저장소 슬롯 좌클릭 처리
    /// </summary>
    private void HandleMainStorageLeftClick(int slotIndex)
    {
        HandleSlotLeftClick(_inventory.MainStorage, slotIndex);
    }
    
    /// <summary>
    /// 메인 저장소 슬롯 우클릭 처리
    /// </summary>
    private void HandleMainStorageRightClick(int slotIndex)
    {
        HandleSlotRightClick(_inventory.MainStorage, slotIndex);
    }
    
    #endregion
    
    #region 퀵 저장소 상호작용
    
    /// <summary>
    /// 퀵 저장소 슬롯 좌클릭 처리
    /// </summary>
    private void HandleQuickStorageLeftClick(int slotIndex)
    {
        HandleSlotLeftClick(_inventory.QuickStorage, slotIndex);
    }
    
    /// <summary>
    /// 퀵 저장소 슬롯 우클릭 처리
    /// </summary>
    private void HandleQuickStorageRightClick(int slotIndex)
    {
        HandleSlotRightClick(_inventory.QuickStorage, slotIndex);
    }
    
    #endregion
    
    #region 공통 슬롯 상호작용 로직
    
    /// <summary>
    /// 슬롯 좌클릭 처리
    /// </summary>
    private void HandleSlotLeftClick(Storage storage, int slotIndex)
    {
        if (storage == null) return;
        
        if (_isHoldingItem)
        {
            PlaceHoldingItem(storage, slotIndex);
        }
        else
        {
            PickupItem(storage, slotIndex);
        }
    }
    
    /// <summary>
    /// 슬롯 우클릭 처리
    /// </summary>
    private void HandleSlotRightClick(Storage storage, int slotIndex)
    {
        if (storage == null) return;
        
        if (_isSplitMode)
        {
            SplitItem(storage, slotIndex);
        }
        else
        {
            PickupOneItem(storage, slotIndex);
        }
    }
    
    /// <summary>
    /// 들고 있는 아이템을 슬롯에 배치
    /// </summary>
    private void PlaceHoldingItem(Storage storage, int slotIndex)
    {
        ItemInstance targetItem = storage.GetItemAt(slotIndex);
        
        // 타겟 슬롯에 아이템이 있는 경우
        if (targetItem != null)
        {
            // 같은 종류의 아이템이면 스택 시도
            if (targetItem.ItemInfo.key == _holdItem.ItemInfo.key)
            {
                TryStackItems(storage, slotIndex);
            }
            else
            {
                // 다른 종류의 아이템이면 교환
                SwapItems(storage, slotIndex);
            }
        }
        else
        {
            // 타겟 슬롯이 비어있으면 배치
            PlaceItemInEmptySlot(storage, slotIndex);
        }
    }
    
    /// <summary>
    /// 아이템 스택 시도
    /// </summary>
    private void TryStackItems(Storage storage, int slotIndex)
    {
        // 슬롯에 아이템 추가 시도
        bool success = storage.TryAddItemAt(slotIndex, _holdItem);
        
        // 모든 아이템을 추가했으면 들고 있는 아이템 해제
        if (success || _holdItem.IsEmpty)
        {
            ClearHoldingItem();
        }
        else
        {
            // 일부만 추가했으면 UI 업데이트
            UpdateHoldingItem(_holdItem);
        }
    }
    
    /// <summary>
    /// 아이템 교환
    /// </summary>
    private void SwapItems(Storage storage, int slotIndex)
    {
        storage.SwapItem(slotIndex, ref _holdItem);
        UpdateHoldingItem(_holdItem);
    }
    
    /// <summary>
    /// 빈 슬롯에 아이템 배치
    /// </summary>
    private void PlaceItemInEmptySlot(Storage storage, int slotIndex)
    {
        storage.SetItemAt(slotIndex, _holdItem);
        ClearHoldingItem();
    }
    
    /// <summary>
    /// 아이템 집기
    /// </summary>
    private void PickupItem(Storage storage, int slotIndex)
    {
        _holdItem = storage.GetItemAt(slotIndex);
        
        if (_holdItem != null)
        {
            SetHoldingItem(true);
            storage.RemoveItemAt(slotIndex);
        }
    }
    
    /// <summary>
    /// 아이템 하나만 집기
    /// </summary>
    private void PickupOneItem(Storage storage, int slotIndex)
    {
        ItemInstance slotItem = storage.GetItemAt(slotIndex);
        
        if (slotItem == null) return;
        
        // 이미 아이템을 들고 있는 경우
        if (_isHoldingItem)
        {
            // 같은 종류 아이템인 경우 1개 추가
            if (_holdItem.ItemInfo.key == slotItem.ItemInfo.key)
            {
                if (_holdItem.AddQuantity(1) == 0)
                {
                    // 성공적으로 추가됨
                    storage.RemoveItemAt(slotIndex, 1);
                    UpdateHoldingItem(_holdItem);
                }
            }
        }
        else
        {
            // 아이템을 들고 있지 않은 경우, 1개를 가져옴
            _holdItem = new ItemInstance(slotItem.ItemInfo, 1);
            
            // 원본 아이템에서 1개 감소
            storage.RemoveItemAt(slotIndex, 1);
            
            // 아이템 들고 있는 상태로 변경
            SetHoldingItem(true);
        }
    }
    
    /// <summary>
    /// 아이템 분할
    /// </summary>
    private void SplitItem(Storage storage, int slotIndex)
    {
        ItemInstance slotItem = storage.GetItemAt(slotIndex);
        
        if (slotItem == null || slotItem.Quantity <= 1) return;
        
        int splitQuantity = slotItem.Quantity / 2;
        
        // 들고 있는 아이템이 없는 경우
        if (!_isHoldingItem)
        {
            // 분할된 아이템으로 새 인스턴스 생성
            _holdItem = new ItemInstance(slotItem.ItemInfo, splitQuantity);
            
            // 원본 아이템에서 수량 감소
            storage.RemoveItemAt(slotIndex, splitQuantity);
            
            // 아이템 들기 상태 설정
            SetHoldingItem(true);
        }
        else if (_holdItem.ItemInfo.key == slotItem.ItemInfo.key)
        {
            // 같은 종류의 아이템이면 최대 스택까지 추가 시도
            int remainder = _holdItem.AddQuantity(splitQuantity);
            
            // 실제로 추가된 수량
            int actualAdded = splitQuantity - remainder;
            
            // 원본 아이템에서 수량 감소
            if (actualAdded > 0)
            {
                storage.RemoveItemAt(slotIndex, actualAdded);
                UpdateHoldingItem(_holdItem);
            }
        }
    }
    
    #endregion
    
    #region 헬퍼 메서드
    
    /// <summary>
    /// 들고 있는 아이템 상태 설정
    /// </summary>
    private void SetHoldingItem(bool isHolding)
    {
        _isHoldingItem = isHolding;
        
        if (_holdItemUI != null)
        {
            _holdItemUI.gameObject.SetActive(_isHoldingItem);
            
            if (_isHoldingItem && _holdItem != null)
            {
                UpdateHoldingItem(_holdItem);
            }
        }
    }
    
    /// <summary>
    /// 들고 있는 아이템 UI 업데이트
    /// </summary>
    private void UpdateHoldingItem(ItemInstance item)
    {
        if (_holdItemUI != null && item != null)
        {
            _holdItemUI.UpdateHoldItem(item);
        }
    }
    
    /// <summary>
    /// 들고 있는 아이템 해제
    /// </summary>
    private void ClearHoldingItem()
    {
        _holdItem = null;
        SetHoldingItem(false);
    }
    
    #endregion
}
