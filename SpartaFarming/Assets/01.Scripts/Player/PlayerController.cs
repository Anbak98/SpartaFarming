using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

/// <summary>
/// 플레이어의 입력 처리와 동작을 관리하는 컴포넌트입니다.
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Serialized Fields
    [Header("인벤토리")]
    [SerializeField] private PlayerQuickslot m_quickSlot;
    [SerializeField] private GameObject m_inventoryUI;
    [SerializeField] private Inventory m_inventory;

    [Header("상태 머신")]
    [SerializeField] private PlayerStateMachine m_stateMachine;

    [Header("도구 피벗")]
    [SerializeField] public GameObject m_toolPivot;

    [Header("도구 오브젝트")]
    [SerializeField] private GameObject m_axe;
    [SerializeField] private GameObject m_hoe;
    [SerializeField] private GameObject m_fishingRod;
    [SerializeField] private GameObject m_wateringCan;
    [SerializeField] private GameObject m_pickAxe;
    [SerializeField] private GameObject m_seed;

    [Header("타일맵")]
    [SerializeField] public Tilemap m_objectMap;
    [SerializeField] public Tilemap m_floorMap;
    [SerializeField] public Tilemap m_waterMap;
    [SerializeField] public Tilemap m_oreMap;
    [SerializeField] public TileBase m_floorTile;

    [Header("펜스 아이템")]
    [SerializeField] public FenceToolUI m_fenceToolUI;
    [SerializeField] public List<FenceToolSlot> m_fenceToolSlots;
    [SerializeField] public List<TileBase> m_fences;
    #endregion

    #region Public Properties
    /// <summary>
    /// 플레이어의 인벤토리에 대한 접근자입니다.
    /// </summary>
    public Inventory Inventory 
    { 
        get => m_inventory; 
        set => m_inventory = value; 
    }

    /// <summary>
    /// 플레이어가 NPC와 상호작용 가능한지 여부를 나타냅니다.
    /// </summary>
    public bool CanInteract { get; set; } = false;
    
    /// <summary>
    /// 마지막으로 이동한 X 방향입니다.
    /// </summary>
    public float LastMoveX { get; private set; }
    
    /// <summary>
    /// 마지막으로 이동한 Y 방향입니다.
    /// </summary>
    public float LastMoveY { get; private set; }
    #endregion

    #region Public Events
    /// <summary>
    /// 광석을 채굴할 때 발생하는 이벤트입니다.
    /// </summary>
    public Action<Vector3Int> OnMine;
    
    /// <summary>
    /// 펜스를 제거할 때 발생하는 이벤트입니다.
    /// </summary>
    public Action OnRemoveFence;
    
    /// <summary>
    /// 펜스를 설치할 때 발생하는 이벤트입니다.
    /// </summary>
    public Action OnPlaceFence;
    
    /// <summary>
    /// 땅을 경작할 때 발생하는 이벤트입니다.
    /// </summary>
    public Action OnHoeing;
    #endregion

    #region Private Fields
    private float m_currentSpeed = 0f;
    private float m_moveSpeed = 3f;
    private float m_runSpeed = 5f;
    private bool m_isMoving = false;
    private Vector2 m_currentMovementInput;
    private bool m_isEquipped = false;
    private bool m_isNearWater = false;
    private GameObject m_currentTool;
    private float m_horizontal;
    private float m_vertical;
    private Vector2 m_mousePosition;
    private Rigidbody2D m_rigidbody;
    private Animator m_playerAnimator;
    private Animator m_toolAnimator;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// 컴포넌트 초기화 시 호출됩니다.
    /// </summary>
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_playerAnimator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// 매 프레임마다 호출됩니다.
    /// </summary>
    private void Update()
    {
        SetMousePosition();
    }

    /// <summary>
    /// 물리 타임스텝마다 호출됩니다.
    /// </summary>
    private void FixedUpdate()
    {
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_vertical = Input.GetAxisRaw("Vertical");

        Move();
        m_playerAnimator.speed = m_currentSpeed / m_moveSpeed;

        SetMoveRotAnime();
        GetCurrentToolAnimation();
    }

    /// <summary>
    /// 다른 콜라이더가 트리거 영역 내에 머무를 때 호출됩니다.
    /// </summary>
    /// <param name="other">충돌한 콜라이더</param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) 
        {
            m_isNearWater = true;
        }

        if (m_currentTool != null && m_currentTool.CompareTag("Rod"))
        {
            if (m_isMoving)
            {
                if (m_toolAnimator != null) 
                {
                    m_toolAnimator.SetBool("Fishing", false);
                }
                Destroy(m_currentTool);
                m_toolAnimator = null;
                m_isEquipped = false;
            }
        }
    }

    /// <summary>
    /// 다른 콜라이더가 트리거 영역을 벗어날 때 호출됩니다.
    /// </summary>
    /// <param name="other">충돌한 콜라이더</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            m_isNearWater = false;

            if (m_currentTool != null && m_currentTool.CompareTag("Rod"))
            {
                m_toolAnimator.SetBool("Fishing", false);
                Destroy(m_currentTool);
                m_toolAnimator = null;
                m_isEquipped = false;
            }
        }
    }
    #endregion

    #region Public Methods - Input Actions
    /// <summary>
    /// 마우스 위치를 월드 좌표로 설정합니다.
    /// </summary>
    public void SetMousePosition()
    {
        m_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    /// <summary>
    /// 단축키 입력을 처리합니다.
    /// </summary>
    /// <param name="context">입력 콜백 컨텍스트</param>
    public void OnHotkeyPressed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            string key = context.control.name;

            // 특수 키 처리
            switch (key)
            {
                case "0":
                    key = "10"; break;
                case "minus":
                    key = "11"; break;
                case "equals":
                    key = "12"; break;
            }

            if (int.TryParse(key, out int index))
            {
                ItemInstance itemInstance = m_quickSlot.SetAndGet(index);

                // 기존 장착 상태 초기화
                m_isEquipped = false;
                m_toolAnimator = null;
                m_axe.SetActive(false);
                m_hoe.SetActive(false);
                m_fishingRod.SetActive(false);
                m_wateringCan.SetActive(false);
                m_pickAxe.SetActive(false);
                m_fenceToolUI.gameObject.SetActive(false);

                // 선택한 아이템이 없는 경우 상태 초기화
                if (itemInstance == null)
                {
                    m_stateMachine.ClearState();
                    return;
                }

                // 아이템 유형에 따른 상태 변경
                switch (itemInstance.ItemInfo.itemType)
                {
                    case DesignEnums.ItemType.Seed:
                        m_stateMachine.ChangeState(m_stateMachine.seedingState); 
                        break;
                    case DesignEnums.ItemType.Tool:
                        HandleToolSelection(itemInstance);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 이동 입력을 처리합니다.
    /// </summary>
    /// <param name="context">입력 콜백 컨텍스트</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            m_currentMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            m_currentMovementInput = Vector2.zero;
        }
    }

    /// <summary>
    /// 아이템 획득 입력을 처리합니다.
    /// </summary>
    /// <param name="context">입력 콜백 컨텍스트</param>
    public void OnItemGet(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (TryGetComponent(out ItemObject itemObject))
            {
                itemObject.PickUp(m_inventory);
            }
        }
    }

    /// <summary>
    /// 인벤토리 열기/닫기 입력을 처리합니다.
    /// </summary>
    /// <param name="context">입력 콜백 컨텍스트</param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.playerMenuUI.isOpen && context.phase == InputActionPhase.Started) 
        {
            UIManager.Instance.OpenPlayerMenuUI();
        }
        else if (UIManager.Instance.playerMenuUI.isOpen && context.phase == InputActionPhase.Started) 
        {
            UIManager.Instance.ClosePlayerMenuUI();
        }
    }

    /// <summary>
    /// 도구 장착/해제 입력을 처리합니다.
    /// </summary>
    /// <param name="context">입력 콜백 컨텍스트</param>
    public void OnEquip(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !m_isEquipped)
        {
            m_isEquipped = true;
        }
        else if (context.phase == InputActionPhase.Started && m_isEquipped)
        {
            Destroy(m_currentTool);
            m_toolAnimator = null;
            m_isEquipped = false;
        }
    }

    /// <summary>
    /// 사용 입력을 처리합니다.
    /// </summary>
    /// <param name="context">입력 콜백 컨텍스트</param>
    public void OnUse(InputAction.CallbackContext context)
    {        
        if (context.phase == InputActionPhase.Started)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                m_stateMachine.DoAction();
            }            
        }
    }

    /// <summary>
    /// 상호작용 입력을 처리합니다.
    /// </summary>
    /// <param name="context">입력 콜백 컨텍스트</param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (CanInteract && context.phase == InputActionPhase.Started)
        {
            Debug.Log("NPC에게 말 걸기");
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// 도구 아이템 선택을 처리합니다.
    /// </summary>
    /// <param name="itemInstance">선택된 아이템 인스턴스</param>
    private void HandleToolSelection(ItemInstance itemInstance)
    {
        string itemName = itemInstance.ItemInfo.name;
        
        switch(itemName)
        {
            case "PickAxe":
                m_stateMachine.ChangeState(m_stateMachine.miningState);
                m_pickAxe.SetActive(true);
                m_isEquipped = true;
                break;
            case "Sword":
                m_stateMachine.ChangeState(m_stateMachine.miningState);
                m_axe.SetActive(true);
                m_isEquipped = true;
                break;
            case "FishingRod":
                m_stateMachine.ChangeState(m_stateMachine.fishingState);
                m_fishingRod.SetActive(true);
                m_isEquipped = true;
                break;
            case "Hoe":
                m_stateMachine.ChangeState(m_stateMachine.hoeingState);
                m_hoe.SetActive(true);
                m_isEquipped = true;
                break;
            case "Axe":
                m_stateMachine.ChangeState(m_stateMachine.removingFenceState);
                m_axe.SetActive(true);
                m_isEquipped = true;
                break;
            case "Fence":
                m_stateMachine.ChangeState(m_stateMachine.placingFenceState);
                m_fenceToolUI.gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 플레이어 이동 시 애니메이션을 업데이트합니다.
    /// </summary>
    private void SetMoveRotAnime()
    {
        // 현재 이동 방향에 따른 애니메이션 설정
        m_playerAnimator.SetFloat("Horizontal", m_horizontal);
        m_playerAnimator.SetFloat("Vertical", m_vertical);

        // 도구 애니메이션이 있는 경우에도 동일하게 적용
        if (m_toolAnimator != null)
        {
            m_toolAnimator.SetFloat("Horizontal", m_horizontal);
            m_toolAnimator.SetFloat("Vertical", m_vertical);
        }

        // 이동 방향이 있는 경우 마지막 이동 방향 저장
        if (m_horizontal != 0 || m_vertical != 0)
        {
            m_playerAnimator.SetFloat("LastMoveX", m_horizontal);
            m_playerAnimator.SetFloat("LastMoveY", m_vertical);

            LastMoveX = m_horizontal;
            LastMoveY = m_vertical;
        }

        // 도구 애니메이션이 있는 경우 마지막 이동 방향 적용
        if (m_toolAnimator != null)
        {
            m_toolAnimator.SetFloat("LastMoveX", LastMoveX);
            m_toolAnimator.SetFloat("LastMoveY", LastMoveY);
        }
    }

    /// <summary>
    /// 현재 활성화된 도구의 애니메이터를 가져옵니다.
    /// </summary>
    private void GetCurrentToolAnimation()
    {        
        // 더 효율적인 방법으로 리팩토링
        for (int i = 0; i < m_toolPivot.transform.childCount; i++)
        {
            GameObject child = m_toolPivot.transform.GetChild(i).gameObject;
            if (child.activeInHierarchy)
            {
                m_toolAnimator = child.GetComponent<Animator>();
                return;
            }
        }
        
        // 모든 자식이 비활성화된 경우 null로 설정
        m_toolAnimator = null;
    }

    /// <summary>
    /// 플레이어 이동을 처리합니다.
    /// </summary>
    private void Move()
    {
        m_isMoving = true;

        // 달리기 키 누르면 달리기 속도, 아니면 걷기 속도 사용
        m_currentSpeed = Input.GetKey(KeyCode.Space) ? m_runSpeed : m_moveSpeed;

        // 이동 방향 계산
        Vector3 direction = transform.up * m_currentMovementInput.y + transform.right * m_currentMovementInput.x;
        direction *= m_currentSpeed;

        // 이동 적용
        m_rigidbody.velocity = direction;

        // 속도가 0이면 이동 중이 아님
        if (m_rigidbody.velocity == Vector2.zero) 
        {
            m_isMoving = false;
        }
    }
    #endregion
}
