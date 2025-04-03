using System;
using UnityEngine;

/// <summary>
/// 게임 내 아이템의 구체적인 인스턴스를 나타내는 클래스
/// </summary>
[Serializable]
public class ItemInstance
{
    // 기본 속성
    [SerializeField] private int _quantity;
    [SerializeField] private float _currentDurability;
    [SerializeField] private bool _isHolding;
    [SerializeField] private bool _isEquipped;

    // 읽기 전용 속성
    public ItemInfo ItemInfo { get; private set; }
    
    // 속성 접근자
    public int Quantity 
    { 
        get => _quantity;
        set => _quantity = Mathf.Max(0, value);
    }
    
    public float CurrentDurability
    {
        get => _currentDurability;
        set => _currentDurability = Mathf.Clamp(value, 0f, ItemInfo.durability);
    }
    
    public bool IsHolding
    {
        get => _isHolding;
        set => _isHolding = value;
    }
    
    public bool IsEquipped
    {
        get => _isEquipped;
        set => _isEquipped = value;
    }

    /// <summary>
    /// 아이템 인스턴스 생성자
    /// </summary>
    /// <param name="itemInfo">아이템 기본 정보</param>
    /// <param name="quantity">수량</param>
    public ItemInstance(ItemInfo itemInfo, int quantity = 1)
    {
        ItemInfo = itemInfo;
        _quantity = Mathf.Max(1, quantity);
        _currentDurability = itemInfo.durability;
        _isHolding = false;
        _isEquipped = false;
    }
    
    /// <summary>
    /// 아이템 복제 생성
    /// </summary>
    /// <param name="sourceItem">복제할 아이템</param>
    /// <param name="quantity">설정할 수량 (기본값은 원본 아이템의 수량)</param>
    public ItemInstance(ItemInstance sourceItem, int? quantity = null)
    {
        ItemInfo = sourceItem.ItemInfo;
        _quantity = quantity ?? sourceItem.Quantity;
        _currentDurability = sourceItem.CurrentDurability;
        _isHolding = false;
        _isEquipped = false;
    }
    
    /// <summary>
    /// 이 아이템과 다른 아이템이 스택 가능한지 확인
    /// </summary>
    /// <param name="other">비교할 다른 아이템</param>
    /// <returns>스택 가능 여부</returns>
    public bool CanStackWith(ItemInstance other)
    {
        if (other == null) return false;
        
        return ItemInfo.key == other.ItemInfo.key && 
               ItemInfo.maxStack > 1 && 
               Quantity < ItemInfo.maxStack;
    }
    
    /// <summary>
    /// 아이템에 수량 추가
    /// </summary>
    /// <param name="amount">추가할 수량</param>
    /// <returns>추가 후 남은 수량 (초과 시)</returns>
    public int AddQuantity(int amount)
    {
        int total = _quantity + amount;
        int max = ItemInfo.maxStack;
        
        if (total <= max)
        {
            _quantity = total;
            return 0;
        }
        else
        {
            _quantity = max;
            return total - max;
        }
    }
    
    /// <summary>
    /// 아이템에서 수량 감소
    /// </summary>
    /// <param name="amount">감소시킬 수량</param>
    /// <returns>실제로 감소된 수량</returns>
    public int RemoveQuantity(int amount)
    {
        int actualAmount = Mathf.Min(amount, _quantity);
        _quantity -= actualAmount;
        return actualAmount;
    }
    
    /// <summary>
    /// 아이템이 비어있는지 확인 (수량이 0인지)
    /// </summary>
    public bool IsEmpty => _quantity <= 0;
}
