using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float curSpeed;
    public float moveSpeed;
    public float runSpeed;
    public bool isMoving = false;
    
    public Vector2 curMovementInput;    
    private float horizontal;
    private float vertical;

    public float plLastMoveX;
    public float plLastMoveY;

    private Vector2 mousePosition;

    private Rigidbody2D _rigidbody;
    private Animator playerAnimator;
    private Animator toolAnimator;    

    [Header("Equipment")]
    public bool equipped = false;
    public bool nearWater = false;
    public EquipItemUI equipItemUI;
    public int selectedEquipItemIndex;
    public List<GameObject> equipTools;
    public GameObject curTool;
    public GameObject toolPivot;
    
    public GameObject axeTool;
    public GameObject harvestTool;
    public GameObject wateringTool;
    public GameObject fishingTool;    

    [Header("MapManagement")]
    public FenceToolUI fenceToolUI;
    public List<FenceToolSlot> fenceToolSlots;
    public Tilemap objectMap;
    public List<TileBase> fences;

    public Tilemap floorMap;    
    public TileBase floorTile;

    public Tilemap waterMap;

    public GameObject inventoryUI;
    public Inventory inventory;

    public Action<Vector3Int> onMine;
    public Action onRemoveFence;
    public Action onPlaceFence;
    public Action onHoeing;

    public PlayerStateMachine playerStateMachine;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponentInChildren<Animator>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        SetMousePosition();
        GetEquipItemIndex();
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Move();
        playerAnimator.speed = curSpeed / moveSpeed;

        SetMoveRotAnime();
        GetCurToolAnimation();
    }

    // 마우스 포지션 세팅
    public void SetMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }

    // 플레이어 이동
    void Move()
    {
        isMoving = true;

        curSpeed = Input.GetKey(KeyCode.Space) ? runSpeed : moveSpeed;

        Vector3 dir = transform.up * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= curSpeed;

        _rigidbody.velocity = dir;

        if (_rigidbody.velocity == Vector2.zero) isMoving = false;        
    }    

    // Move InputAction
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    // 플레이어 이동 시 애니메이션 활성화, 이동 중단 시 직전 방향의 모습으로 Idle
    void SetMoveRotAnime()
    {
        playerAnimator.SetFloat("Horizontal", horizontal);
        playerAnimator.SetFloat("Vertical", vertical);         

        if (toolAnimator != null)
        {
            toolAnimator.SetFloat("Horizontal", horizontal);
            toolAnimator.SetFloat("Vertical", vertical);
        }

        if (horizontal == 1 || horizontal == -1 || vertical == 1 || vertical == -1)
        {
            playerAnimator.SetFloat("LastMoveX", horizontal);
            playerAnimator.SetFloat("LastMoveY", vertical);

            plLastMoveX = horizontal;
            plLastMoveY = vertical;            
        }

        if (toolAnimator != null)
        {
            toolAnimator.SetFloat("LastMoveX", plLastMoveX);
            toolAnimator.SetFloat("LastMoveY", plLastMoveY);
        }
    }

    // curTool Animation GetComponent
    void GetCurToolAnimation()
    {
        if (toolPivot.transform.childCount == 0) return;
        else if (toolPivot.transform.GetChild(0) != null) toolAnimator = toolPivot.transform.GetChild(0).GetComponent<Animator>();        
    }

    public void GetEquipItemIndex()
    {
        for (int i = 0; i < equipItemUI.itemSlots.Length; i++)
        {
            if (equipItemUI.itemSlots[i].isSelected) selectedEquipItemIndex = i;            
        }        
    }

    // ItemGet InputAction
    public void OnItemGet(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (TryGetComponent(out ItemObject itemObject))
            {
                itemObject.PickUp(inventory);
            }
        }
    }

    // Inventory InputAction
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (!inventoryUI.activeInHierarchy && context.phase == InputActionPhase.Started) inventoryUI.SetActive(true);
        else if (inventoryUI.activeInHierarchy && context.phase == InputActionPhase.Started) inventoryUI.SetActive(false);
    }

    //Equip InputAction
    public void OnEquip(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !equipped)
        {
            curTool = Instantiate(equipTools[selectedEquipItemIndex], toolPivot.transform);
            equipped = true;
            EnterState();
        }
        else if (context.phase == InputActionPhase.Started && equipped)
        {
            Destroy(curTool);
            toolAnimator = null;
            equipped = false;
            ExitState();
        }
    }

    // Use InputAction
    public void OnUse(InputAction.CallbackContext context)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 pos = transform.position;
            Vector3Int objGridPos = objectMap.WorldToCell(pos);
            Vector3Int FlrGridPos = floorMap.WorldToCell(pos);

            // 장비 장착과 사용
            if (equipped && !nearWater && context.phase == InputActionPhase.Started)
            {
                playerAnimator.SetTrigger("Use");
                if (curTool.CompareTag("Axe") || curTool.CompareTag("Hoe") || curTool.CompareTag("WateringCan")) toolAnimator.SetTrigger("Use");

                // Axe 플레이어 이전 이동방향 따라 앞의 타일맵 오브젝트 파괴
                if (curTool.CompareTag("Axe")) onRemoveFence?.Invoke();                

                // Hoe 플레이어 이전 이동방향 따라 앞의 땅 파기
                if (curTool.CompareTag("Hoe")) onHoeing?.Invoke();                
            }

            if (equipped && nearWater && context.phase == InputActionPhase.Started)
            {
                playerAnimator.SetTrigger("Use");
                if (curTool.CompareTag("Rod")) toolAnimator.SetBool("Fishing", true);
                else toolAnimator.SetTrigger("Use");
            }

            // FenceTool 들어갔을 때
            if (fenceToolUI.gameObject.activeInHierarchy) onPlaceFence?.Invoke();                 
        }
    }

    // EquipQuickSlot InputAction
    public void OnQuickSlot(InputAction.CallbackContext context)
    {
        if (!equipItemUI.gameObject.activeInHierarchy && !fenceToolUI.gameObject.activeInHierarchy && context.phase == InputActionPhase.Started)
            equipItemUI.gameObject.SetActive(true);
        else if (equipItemUI.gameObject.activeInHierarchy && !fenceToolUI.gameObject.activeInHierarchy && context.phase == InputActionPhase.Started)
        {
            equipItemUI.gameObject.SetActive(false);
            fenceToolUI.gameObject.SetActive(true);
            playerStateMachine.ChangeState(playerStateMachine.placingFenceState);
        }
        else if(!equipItemUI.gameObject.activeInHierarchy && fenceToolUI.gameObject.activeInHierarchy && context.phase == InputActionPhase.Started)
        {
            fenceToolUI.gameObject.SetActive(false);
            ExitState();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) nearWater = true;

        if (curTool != null && curTool.CompareTag("Rod"))
        {
            if (isMoving)
            {
                if (toolAnimator != null) toolAnimator.SetBool("Fishing", false);
                Destroy(curTool);
                toolAnimator = null;
                equipped = false;
            }            
        } 
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            nearWater = false;

            if (curTool != null && curTool.CompareTag("Rod"))
            {
                toolAnimator.SetBool("Fishing", false);
                Destroy(curTool);
                toolAnimator = null;
                equipped = false;
            }
        }
    }

    void EnterState()
    {
        if (curTool == null) return;

        if (curTool.CompareTag("Axe")) playerStateMachine.ChangeState(playerStateMachine.removingFenceState);        
        if (curTool.CompareTag("Hoe")) playerStateMachine.ChangeState(playerStateMachine.hoeingState);
    }

    void ExitState()
    {
        playerStateMachine.ExitState();
    }
}
