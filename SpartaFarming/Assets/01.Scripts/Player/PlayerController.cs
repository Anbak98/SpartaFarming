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

    private float plLastMoveX;
    private float plLastMoveY;

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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponentInChildren<Animator>();        
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
            if (TryGetComponent<ItemObject>(out ItemObject itemObject))
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
                if (curTool.CompareTag("Axe"))
                {
                    if (plLastMoveX == 1) objectMap.SetTile(objGridPos + Vector3Int.right, null);
                    else if (plLastMoveX == -1) objectMap.SetTile(objGridPos + Vector3Int.left, null);
                    else if (plLastMoveY == 1) objectMap.SetTile(objGridPos + Vector3Int.up, null);
                    else if (plLastMoveY == -1) objectMap.SetTile(objGridPos + Vector3Int.down, null);
                    else return;
                }

                // Hoe 플레이어 이전 이동방향 따라 앞의 땅 파기
                if (curTool.CompareTag("Hoe"))
                {
                    if (plLastMoveX == 1 && !objectMap.HasTile(objGridPos + Vector3Int.right) && !waterMap.HasTile(objGridPos + Vector3Int.right))
                        floorMap.SetTile(FlrGridPos + Vector3Int.right, floorTile);
                    else if (plLastMoveX == -1 && !objectMap.HasTile(objGridPos + Vector3Int.left) && !waterMap.HasTile(objGridPos + Vector3Int.left))
                        floorMap.SetTile(FlrGridPos + Vector3Int.left, floorTile);
                    else if (plLastMoveY == 1 && !objectMap.HasTile(objGridPos + Vector3Int.up) && !waterMap.HasTile(objGridPos + Vector3Int.up))
                        floorMap.SetTile(FlrGridPos + Vector3Int.up, floorTile);
                    else if (plLastMoveY == -1 && !objectMap.HasTile(objGridPos + Vector3Int.down) && !waterMap.HasTile(objGridPos + Vector3Int.down))
                        floorMap.SetTile(FlrGridPos + Vector3Int.down, floorTile);
                    else return;
                }
            }

            if (equipped && nearWater && context.phase == InputActionPhase.Started)
            {
                playerAnimator.SetTrigger("Use");
                if (curTool.CompareTag("Rod")) toolAnimator.SetBool("Fishing", true);
                else toolAnimator.SetTrigger("Use");
            }

            // FenceTool 들어갔을 때
            if (fenceToolUI.gameObject.activeInHierarchy)
            {
                // 선택한 fence 플레이어 이전 이동방향 따라 앞에 타일맵에 그리기
                for (int i = 0; i < fenceToolSlots.Count; i++)
                {
                    if (fenceToolSlots[i].isSelected)
                    {
                        if (plLastMoveX == 1 && !objectMap.HasTile(objGridPos + Vector3Int.right) && !waterMap.HasTile(objGridPos + Vector3Int.right))
                            objectMap.SetTile(objGridPos + Vector3Int.right, fences[i]);
                        else if (plLastMoveX == -1 && !objectMap.HasTile(objGridPos + Vector3Int.left) && !waterMap.HasTile(objGridPos + Vector3Int.left))
                            objectMap.SetTile(objGridPos + Vector3Int.left, fences[i]);
                        else if (plLastMoveY == 1 && !objectMap.HasTile(objGridPos + Vector3Int.up) && !waterMap.HasTile(objGridPos + Vector3Int.up))
                            objectMap.SetTile(objGridPos + Vector3Int.up, fences[i]);
                        else if (plLastMoveY == -1 && !objectMap.HasTile(objGridPos + Vector3Int.down) && !waterMap.HasTile(objGridPos + Vector3Int.down))
                            objectMap.SetTile(objGridPos + Vector3Int.down, fences[i]);
                        else return;
                    } 
                }
            }            
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
        }
        else if(!equipItemUI.gameObject.activeInHierarchy && fenceToolUI.gameObject.activeInHierarchy && context.phase == InputActionPhase.Started)
            fenceToolUI.gameObject.SetActive(false);
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
}
