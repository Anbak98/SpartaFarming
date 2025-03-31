using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템을 저장하고 관리하는 저장소 클래스
/// </summary>
[Serializable]
public class Storage
{
    // 저장소 속성
    [SerializeField] private List<ItemInstance> _items = new List<ItemInstance>();
    [SerializeField] private int _maxCount;
    
    // 이벤트
    public event Action<Storage> onChangeStorage;
    
    // 읽기 전용 속성
    public IReadOnlyList<ItemInstance> Items => _items;
    public int MaxCount => _maxCount;
    public int EmptySlotsCount => CountEmptySlots();
    public bool IsFull => EmptySlotsCount == 0;
    
    /// <summary>
    /// 저장소 생성자
    /// </summary>
    /// <param name="maxCount">최대 슬롯 수</param>
    public Storage(int maxCount)
    {
        _maxCount = maxCount;
        InitializeItems();
    }

    /// <summary>
    /// 아이템 슬롯 초기화
    /// </summary>
    private void InitializeItems()
    {
        _items.Clear();
        for (int i = 0; i < _maxCount; i++)
        {
            _items.Add(null);
        }
    }
    
    /// <summary>
    /// 인덱스로 아이템 가져오기
    /// </summary>
    /// <param name="index">슬롯 인덱스</param>
    /// <returns>슬롯의 아이템(없으면 null)</returns>
    public ItemInstance GetItemAt(int index)
    {
        if (index < 0 || index >= _maxCount) return null;
        return _items[index];
    }
    
    /// <summary>
    /// 인덱스에 아이템 설정
    /// </summary>
    /// <param name="index">슬롯 인덱스</param>
    /// <param name="item">설정할 아이템</param>
    /// <returns>성공 여부</returns>
    public bool SetItemAt(int index, ItemInstance item)
    {
        if (index < 0 || index >= _maxCount) return false;
        
        _items[index] = item;
        NotifyStorageChanged();
        return true;
    }

    /// <summary>
    /// 아이템 자동 추가
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    /// <returns>추가 성공 여부</returns>
    public bool AddItem(ItemInstance item)
    {
        if (item == null) return false;
        
        // 이미 있는 아이템 스택에 추가 시도
        if (TryStackExistingItems(item))
            return true;

        // 빈 슬롯에 배치 시도
        return PlaceInEmptySlots(item);
    }

    /// <summary>
    /// 기존 아이템 스택에 추가 시도
    /// </summary>
    private bool TryStackExistingItems(ItemInstance item)
    {
        if (item == null || item.ItemInfo.maxStack <= 1)
            return false;

        for (int i = 0; i < _maxCount; i++)
        {
            if (_items[i] != null && _items[i].CanStackWith(item))
            {
                int remainder = _items[i].AddQuantity(item.Quantity);
                
                if (remainder == 0)
                {
                    // 모든 수량이 추가됨
                    NotifyStorageChanged();
                    return true;
                }
                else
                {
                    // 일부만 추가됨, 남은 수량 업데이트
                    item.Quantity = remainder;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 빈 슬롯에 아이템 배치
    /// </summary>
    private bool PlaceInEmptySlots(ItemInstance item)
    {
        if (item == null) return false;
        
        List<int> emptySlots = FindEmptySlots();
        
        if (emptySlots.Count == 0) return false;
        
        int remainingQuantity = item.Quantity;
        int maxStackSize = item.ItemInfo.maxStack;
        
        foreach (int slotIndex in emptySlots)
        {
            if (remainingQuantity <= 0) break;
            
            int stackSize = Mathf.Min(remainingQuantity, maxStackSize);
            
            // 새 아이템 인스턴스 생성
            ItemInstance newStackItem = new ItemInstance(item.ItemInfo, stackSize);
            _items[slotIndex] = newStackItem;
            
            remainingQuantity -= stackSize;
        }
        
        // 원본 아이템의 수량 업데이트
        item.Quantity = remainingQuantity;
        
        NotifyStorageChanged();
        return remainingQuantity <= 0;
    }

    /// <summary>
    /// 특정 슬롯에 아이템 추가 시도
    /// </summary>
    /// <param name="index">슬롯 인덱스</param>
    /// <param name="item">추가할 아이템</param>
    /// <returns>추가 성공 여부</returns>
    public bool TryAddItemAt(int index, ItemInstance item)
    {
        if (index < 0 || index >= _maxCount || item == null) return false;
        
        // 빈 슬롯인 경우
        if (_items[index] == null)
        {
            _items[index] = item;
            NotifyStorageChanged();
            return true;
        }
        
        // 같은 종류 아이템인 경우 스택 시도
        if (_items[index].CanStackWith(item))
        {
            int remainder = _items[index].AddQuantity(item.Quantity);
            item.Quantity = remainder;
            
            NotifyStorageChanged();
            return remainder == 0;
        }
        
        return false;
    }

    /// <summary>
    /// 비어있는 슬롯 인덱스 찾기
    /// </summary>
    private List<int> FindEmptySlots()
    {
        List<int> emptySlots = new List<int>();
        
        for (int i = 0; i < _maxCount; i++)
        {
            if (_items[i] == null)
            {
                emptySlots.Add(i);
            }
        }
        
        return emptySlots;
    }
    
    /// <summary>
    /// 빈 슬롯 개수 계산
    /// </summary>
    private int CountEmptySlots()
    {
        int count = 0;
        
        for (int i = 0; i < _maxCount; i++)
        {
            if (_items[i] == null)
            {
                count++;
            }
        }
        
        return count;
    }

    /// <summary>
    /// 특정 키를 가진 아이템 제거
    /// </summary>
    /// <param name="key">아이템 키</param>
    /// <param name="quantity">제거할 수량</param>
    public void RemoveItem(int key, ref int quantity)
    {
        if (quantity <= 0) return;
        
        for (int i = 0; i < _maxCount; i++)
        {
            if (_items[i] == null) continue;
            
            if (_items[i].ItemInfo.key == key)
            {
                int removed = _items[i].RemoveQuantity(quantity);
                quantity -= removed;
                
                // 수량이 0이 되면 슬롯에서 제거
                if (_items[i].IsEmpty)
                {
                    _items[i] = null;
                }
                
                if (quantity <= 0) break;
            }
        }
        
        NotifyStorageChanged();
    }

    /// <summary>
    /// 특정 슬롯에서 아이템 제거
    /// </summary>
    /// <param name="index">슬롯 인덱스</param>
    public void RemoveItemAt(int index)
    {
        if (index < 0 || index >= _maxCount) return;
        
        _items[index] = null;
        NotifyStorageChanged();
    }

    /// <summary>
    /// 특정 슬롯에서 일정 수량 제거
    /// </summary>
    /// <param name="index">슬롯 인덱스</param>
    /// <param name="quantity">제거할 수량</param>
    /// <returns>실제 제거된 수량</returns>
    public int RemoveItemAt(int index, int quantity)
    {
        if (index < 0 || index >= _maxCount || _items[index] == null) return 0;
        
        int removed = _items[index].RemoveQuantity(quantity);
        
        if (_items[index].IsEmpty)
        {
            _items[index] = null;
        }
        
        NotifyStorageChanged();
        return removed;
    }

    /// <summary>
    /// 아이템 슬롯 교환
    /// </summary>
    /// <param name="index">대상 슬롯 인덱스</param>
    /// <param name="item">교환할 아이템 (참조로 전달)</param>
    /// <returns>교환 성공 여부</returns>
    public bool SwapItem(int index, ref ItemInstance item)
    {
        if (index < 0 || index >= _maxCount) return false;
        
        ItemInstance temp = _items[index];
        _items[index] = item;
        item = temp;
        
        NotifyStorageChanged();
        return true;
    }
    
    /// <summary>
    /// 두 슬롯 간 아이템 교환
    /// </summary>
    /// <param name="fromIndex">시작 슬롯 인덱스</param>
    /// <param name="toIndex">대상 슬롯 인덱스</param>
    /// <returns>교환 성공 여부</returns>
    public bool SwapSlots(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= _maxCount || 
            toIndex < 0 || toIndex >= _maxCount || 
            fromIndex == toIndex)
        {
            return false;
        }
        
        ItemInstance temp = _items[fromIndex];
        _items[fromIndex] = _items[toIndex];
        _items[toIndex] = temp;
        
        NotifyStorageChanged();
        return true;
    }
    
    /// <summary>
    /// 특정 키를 가진 아이템이 있는지 확인
    /// </summary>
    /// <param name="key">아이템 키</param>
    /// <returns>해당 아이템의 총 수량</returns>
    public int GetItemCount(int key)
    {
        int count = 0;
        
        for (int i = 0; i < _maxCount; i++)
        {
            if (_items[i] != null && _items[i].ItemInfo.key == key)
            {
                count += _items[i].Quantity;
            }
        }
        
        return count;
    }

    /// <summary>
    /// 저장소 변경 이벤트 발생
    /// </summary>
    private void NotifyStorageChanged()
    {
        onChangeStorage?.Invoke(this);
    }
}
