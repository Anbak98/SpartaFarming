using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private PlayerQuickslot _quickSlot;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private Inventory inventory;
    public Inventory Inventory { get { return inventory; } set { inventory = value; } }

    [Header("StateMachine")]
    [SerializeField] private PlayerStateMachine _stateMachine;

    [Header("Hand Pivot")]
    [SerializeField] public GameObject toolPivot;

    [Header("Hand Objects")]
    [SerializeField] private GameObject _axe;
    [SerializeField] private GameObject _hoe;
    [SerializeField] private GameObject _fishingRod;
    [SerializeField] private GameObject _wateringCan;
    [SerializeField] private GameObject _pickAxe;
    [SerializeField] private GameObject _seed;

    // 마우스 포지션 세팅
    public void SetMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }

    public void OnHotkeyPressed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            string key = context.control.name;

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
                ItemInstance itemInstance = _quickSlot.SetAndGet(index);

                equipped = false;
                toolAnimator = null;
                _axe.SetActive(false);
                _hoe.SetActive(false);
                _fishingRod.SetActive(false);
                _wateringCan.SetActive(false);
                _pickAxe.SetActive(false);
                fenceToolUI.gameObject.SetActive(false);

                if (itemInstance == null)
                {
                    _stateMachine.ClearState();
                    return;
                }

                switch (itemInstance.ItemInfo.itemType)
                {
                    case DesignEnums.ItemType.Seed:
                        _stateMachine.ChangeState(_stateMachine.seedingState); break;
                    case DesignEnums.ItemType.Tool:
                        if (itemInstance.ItemInfo.name == "PickAxe")
                        {
                            _stateMachine.ChangeState(_stateMachine.miningState);
                            _pickAxe.SetActive(true);
                            equipped = true;
                        }
                        else if (itemInstance.ItemInfo.name == "Sword")
                        {
                            _stateMachine.ChangeState(_stateMachine.miningState);
                            _axe.SetActive(true);
                            equipped = true;
                        }
                        else if (itemInstance.ItemInfo.name == "FishingRod")
                        {
                            _stateMachine.ChangeState(_stateMachine.fishingState);
                            _fishingRod.SetActive(true);
                            equipped = true;
                        }
                        else if (itemInstance.ItemInfo.name == "Hoe")
                        {
                            _stateMachine.ChangeState(_stateMachine.hoeingState);
                            _hoe.SetActive(true);
                            equipped = true;
                        }
                        else if (itemInstance.ItemInfo.name == "Axe")
                        {
                            _stateMachine.ChangeState(_stateMachine.removingFenceState);
                            _axe.SetActive(true);
                            equipped = true;
                        }
                        else if (itemInstance.ItemInfo.name == "Fence")
                        {
                            _stateMachine.ChangeState(_stateMachine.placingFenceState);
                            fenceToolUI.gameObject.SetActive(true);
                        }
                        break;
                }
            }
        }
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
            //curTool = Instantiate(equipTools[selectedEquipItemIndex], toolPivot.transform);
            equipped = true;
        }
        else if (context.phase == InputActionPhase.Started && equipped)
        {
            Destroy(curTool);
            toolAnimator = null;
            equipped = false;
        }
    }

    // Use InputAction
    public void OnUse(InputAction.CallbackContext context)
    {        
        if (context.phase == InputActionPhase.Started)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _stateMachine.DoAction();
            }            
        }
    }

    #region Life Cycle
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        SetMousePosition();
        //GetEquipItemIndex();
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
    #endregion

    #region Private Field
    private float curSpeed = 0;
    private float moveSpeed = 3;
    private float runSpeed = 5;
    private bool isMoving = false;
    private Vector2 curMovementInput;
    private bool equipped = false;
    private bool nearWater = false;
    private GameObject curTool;
    private float horizontal;
    private float vertical;

    public float plLastMoveX;
    public float plLastMoveY;

    private Vector2 mousePosition;

    private Rigidbody2D _rigidbody;
    private Animator playerAnimator;
    private Animator toolAnimator;

    public FenceToolUI fenceToolUI;
    public List<FenceToolSlot> fenceToolSlots;
    public Tilemap objectMap;
    public List<TileBase> fences;

    public Tilemap floorMap;
    public TileBase floorTile;

    public Tilemap waterMap;
    public Tilemap oreMap;

    public Action<Vector3Int> onMine;
    public Action onRemoveFence;
    public Action onPlaceFence;
    public Action onHoeing;
    
    #endregion

    #region Private Method
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
        if (toolPivot.transform.GetChild(0).gameObject.activeInHierarchy)
            toolAnimator = toolPivot.transform.GetChild(0).GetComponent<Animator>();
        else if (toolPivot.transform.GetChild(1).gameObject.activeInHierarchy)
            toolAnimator = toolPivot.transform.GetChild(1).GetComponent<Animator>();
        else if (toolPivot.transform.GetChild(2).gameObject.activeInHierarchy)
            toolAnimator = toolPivot.transform.GetChild(2).GetComponent<Animator>();
        else if (toolPivot.transform.GetChild(3).gameObject.activeInHierarchy)
            toolAnimator = toolPivot.transform.GetChild(3).GetComponent<Animator>();
        else if (toolPivot.transform.GetChild(4).gameObject.activeInHierarchy)
            toolAnimator = toolPivot.transform.GetChild(4).GetComponent<Animator>();
    }

    // 플레이어 이동
    private void Move()
    {
        isMoving = true;

        curSpeed = Input.GetKey(KeyCode.Space) ? runSpeed : moveSpeed;

        Vector3 dir = transform.up * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= curSpeed;

        _rigidbody.velocity = dir;

        if (_rigidbody.velocity == Vector2.zero) isMoving = false;
    }

    private void OnTriggerStay2D(Collider2D other)
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

    private void OnTriggerExit2D(Collider2D other)
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
    #endregion
}
