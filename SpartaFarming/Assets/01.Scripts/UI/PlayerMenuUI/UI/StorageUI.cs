using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 저장소의 UI 표현을 관리하는 컴포넌트
/// </summary>
public class StorageUI : MonoBehaviour
{
    [Header("슬롯 설정")]
    [SerializeField] private StorageType _storageType = StorageType.Normal;
    
    // 슬롯 참조 목록
    private List<ItemSlotUI> _itemSlots = new List<ItemSlotUI>();
    
    // 이벤트
    public event Action<int> onSlotLeftClicked;
    public event Action<int> onSlotRightClicked;
    
    // 속성
    public StorageType StorageType => _storageType;
    public IReadOnlyList<ItemSlotUI> ItemSlots => _itemSlots;
    
    /// <summary>
    /// 초기화
    /// </summary>
    private void Awake()
    {
        InitializeSlots();
    }
    
    /// <summary>
    /// 슬롯 초기화
    /// </summary>
    private void InitializeSlots()
    {
        _itemSlots.Clear();
        
        // 자식 오브젝트에서 슬롯 컴포넌트 가져오기
        ItemSlotUI[] slots = GetComponentsInChildren<ItemSlotUI>();
        
        // 슬롯 인덱스 설정 및 리스트에 추가
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SlotIndex = i;
            _itemSlots.Add(slots[i]);
        }
    }
    
    /// <summary>
    /// 저장소 변경에 따라 UI 업데이트
    /// </summary>
    /// <param name="storage">표시할 저장소</param>
    public void UpdateUI(Storage storage)
    {
        if (storage == null || _itemSlots.Count == 0) return;
        
        // 저장소의 아이템 수만큼 슬롯 UI 업데이트
        for (int i = 0; i < storage.Items.Count && i < _itemSlots.Count; i++)
        {
            if (storage.Items[i] != null)
            {
                _itemSlots[i].UpdateUI(storage.Items[i]);
            }
            else
            {
                _itemSlots[i].DisableUI();
            }
        }
    }
    
    /// <summary>
    /// 슬롯 좌클릭 이벤트 처리
    /// </summary>
    /// <param name="slotIndex">슬롯 인덱스</param>
    public void OnSlotLeftClicked(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _itemSlots.Count) return;
        
        onSlotLeftClicked?.Invoke(slotIndex);
    }
    
    /// <summary>
    /// 슬롯 우클릭 이벤트 처리
    /// </summary>
    /// <param name="slotIndex">슬롯 인덱스</param>
    public void OnSlotRightClicked(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _itemSlots.Count) return;
        
        onSlotRightClicked?.Invoke(slotIndex);
    }
}
