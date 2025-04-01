using System;
using UnityEngine;

/// <summary>
/// 게임 월드에 존재하는 아이템 오브젝트
/// </summary>
[Serializable]
public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemInstance _itemInstance;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    /// <summary>
    /// 아이템 인스턴스 속성
    /// </summary>
    public ItemInstance ItemInstance
    {
        get => _itemInstance;
        set
        {
            _itemInstance = value;
            UpdateVisual();
        }
    }
    
    private void Awake()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
    
    private void Start()
    {
        UpdateVisual();
    }
    
    /// <summary>
    /// 아이템 시각적 표현 업데이트
    /// </summary>
    private void UpdateVisual()
    {
        if (_spriteRenderer != null && _itemInstance != null)
        {
            _spriteRenderer.sprite = Resources.Load<Sprite>(_itemInstance.ItemInfo.spritePath);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PickUp(collision.gameObject.GetComponent<Inventory>());
        }
    }

    /// <summary>
    /// 플레이어가 아이템을 획득하는 메서드
    /// </summary>
    /// <param name="inventory">아이템을 추가할 인벤토리</param>
    /// <returns>획득 성공 여부</returns>
    public bool PickUp(Inventory inventory)
    {
        if (inventory == null || _itemInstance == null) return false;
        
        bool result = inventory.AddItem(_itemInstance);
        
        if (result)
        {
            // 아이템 획득 성공 시 오브젝트 제거
            Destroy(gameObject);
        }
        
        return result;
    }
    
    /// <summary>
    /// 아이템 인스턴스를 설정하는 메서드
    /// </summary>
    /// <param name="itemInfo">아이템 정보</param>
    /// <param name="quantity">수량</param>
    public void SetItem(ItemInfo itemInfo, int quantity = 1)
    {
        _itemInstance = new ItemInstance(itemInfo, quantity);
        UpdateVisual();
    }
}
