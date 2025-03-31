using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 관련 입력을 처리하는 컴포넌트
/// </summary>
public class InventoryInputSystem : MonoBehaviour
{
    // 입력 상태
    [SerializeField] private bool _isSplitMode = false;
    
    // 입력 시스템 참조
    private UIInput _uiInput;
    private UIInput.InventoryActions _inventoryActions;
    
    // 이벤트
    public event Action<bool> onSplitModeChanged;
    public event Action onInventoryToggled;
    
    /// <summary>
    /// 현재 분할 모드 여부
    /// </summary>
    public bool IsSplitMode => _isSplitMode;
    
    /// <summary>
    /// 초기화
    /// </summary>
    private void Awake()
    {
        _uiInput = new UIInput();
        _inventoryActions = _uiInput.Inventory;
    }
    
    /// <summary>
    /// 이벤트 연결
    /// </summary>
    private void Start()
    {
        // 분할 모드 이벤트 연결
        _inventoryActions.OnSplit.started += ToggleSplitMode;
        _inventoryActions.OnSplit.canceled += ToggleSplitMode;
    }
    
    /// <summary>
    /// 활성화 시 호출
    /// </summary>
    private void OnEnable()
    {
        _uiInput.Enable();
    }
    
    /// <summary>
    /// 비활성화 시 호출
    /// </summary>
    private void OnDisable()
    {
        _uiInput.Disable();
    }
    
    /// <summary>
    /// 분할 모드 토글
    /// </summary>
    private void ToggleSplitMode(InputAction.CallbackContext context)
    {
        // 눌렀을 때 activated, 뗐을 때 canceled
        _isSplitMode = context.phase == InputActionPhase.Started;
        onSplitModeChanged?.Invoke(_isSplitMode);
    }
    
    /// <summary>
    /// 인벤토리 토글
    /// </summary>
    private void ToggleInventory(InputAction.CallbackContext context)
    {
        onInventoryToggled?.Invoke();
    }
}
