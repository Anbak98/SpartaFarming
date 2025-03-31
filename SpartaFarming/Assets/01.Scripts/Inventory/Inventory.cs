using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 인벤토리를 관리하는 컴포넌트
/// </summary>
public class Inventory : MonoBehaviour
{
    [Header("인벤토리 설정")]
    [SerializeField] private int quickStorageSize = 12;
    [SerializeField] private int mainStorageSize = 24;
    
    // 저장소 객체
    private Storage _quickStorage;
    private Storage _mainStorage;
    
    // 이벤트
    public event Action<ItemInstance> onItemAdded;
    public event Action<int, int> onItemRemoved; // key, quantity
    
    // 속성
    public Storage QuickStorage => _quickStorage;
    public Storage MainStorage => _mainStorage;
    
    /// <summary>
    /// 초기화
    /// </summary>
    private void Awake()
    {
        InitializeStorages();
    }
    
    /// <summary>
    /// 저장소 초기화
    /// </summary>
    private void InitializeStorages()
    {
        _quickStorage = new Storage(quickStorageSize);
        _mainStorage = new Storage(mainStorageSize);
        
        // 저장소 변경 이벤트 리스너 추가
        _quickStorage.onChangeStorage += OnStorageChanged;
        _mainStorage.onChangeStorage += OnStorageChanged;
    }
    
    /// <summary>
    /// 저장소 변경 시 호출되는 메서드
    /// </summary>
    private void OnStorageChanged(Storage storage)
    {
        // 추가 로직이 필요한 경우 여기에 구현
    }
    
    /// <summary>
    /// 아이템 추가 (자동으로 적절한 저장소에 배치)
    /// </summary>
    /// <param name="item">추가할 아이템 인스턴스</param>
    /// <returns>추가 성공 여부</returns>
    public bool AddItem(ItemInstance item)
    {
        if (item == null) return false;
        
        // 퀵 저장소를 먼저 시도하고, 실패하면 메인 저장소에 추가
        bool addedToQuick = _quickStorage.AddItem(item);
        
        if (addedToQuick || item.IsEmpty)
        {
            onItemAdded?.Invoke(item);
            return true;
        }
        
        bool addedToMain = _mainStorage.AddItem(item);
        
        if (addedToMain)
        {
            onItemAdded?.Invoke(item);
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// 새 아이템 생성 후 추가
    /// </summary>
    /// <param name="key">아이템 키</param>
    /// <param name="quantity">수량</param>
    /// <returns>추가 성공 여부</returns>
    public bool AddNewItem(int key, int quantity = 1)
    {
        if (quantity <= 0) return false;
        
        ItemInfo itemInfo = DataManager.ItemLoader.GetByKey(key);
        
        if (itemInfo == null) return false;
        
        ItemInstance newItem = new ItemInstance(itemInfo, quantity);
        return AddItem(newItem);
    }
    
    /// <summary>
    /// 아이템 제거
    /// </summary>
    /// <param name="key">아이템 키</param>
    /// <param name="quantity">제거할 수량</param>
    /// <returns>실제 제거된 수량</returns>
    public int RemoveItem(int key, int quantity = 1)
    {
        if (quantity <= 0) return 0;
        
        int originalQuantity = quantity;
        
        // 퀵 저장소에서 먼저 제거 시도
        _quickStorage.RemoveItem(key, ref quantity);
        
        // 남은 수량이 있으면 메인 저장소에서 제거 시도
        if (quantity > 0)
        {
            _mainStorage.RemoveItem(key, ref quantity);
        }
        
        int removedQuantity = originalQuantity - quantity;
        
        if (removedQuantity > 0)
        {
            onItemRemoved?.Invoke(key, removedQuantity);
        }
        
        return removedQuantity;
    }
    
    /// <summary>
    /// 특정 키를 가진 아이템의 총 수량 확인
    /// </summary>
    /// <param name="key">아이템 키</param>
    /// <returns>인벤토리 내 해당 아이템의 총 수량</returns>
    public int GetItemCount(int key)
    {
        return _quickStorage.GetItemCount(key) + _mainStorage.GetItemCount(key);
    }
    
    /// <summary>
    /// 아이템이 인벤토리에 충분히, 또는 지정 수량 이상 있는지 확인
    /// </summary>
    /// <param name="key">아이템 키</param>
    /// <param name="quantity">확인할 수량</param>
    /// <returns>충분한 수량이 있는지 여부</returns>
    public bool HasItem(int key, int quantity = 1)
    {
        return GetItemCount(key) >= quantity;
    }
}
