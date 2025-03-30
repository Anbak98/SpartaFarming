using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float curSpeed;
    public float moveSpeed;
    public float runSpeed;
    public bool isMoving = false;
    
    private Vector2 curMovementInput;    
    private float horizontal;
    private float vertical;

    private float plLastMoveX;
    private float plLastMoveY;

    private Rigidbody2D _rigidbody;
    private Animator playerAnimator;
    public Animator toolAnimator;
    public bool equipped = false;
    public bool nearWater = false;
    
    public EquipItemUI equipItemUI;
    public GameObject curTool;
    public GameObject toolPivot;
    public List<GameObject> equipTools;
    public GameObject axeTool;
    public GameObject harvestTool;
    public GameObject fishingTool;    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        equipTools = new List<GameObject>(){ axeTool, harvestTool, fishingTool, fishingTool };
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

    // 플레이어 이동
    void Move()
    {
        isMoving = true;

        curSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

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

    //Equip InputAction
    public void OnEquip(InputAction.CallbackContext context)
    {
        for (int i = 0; i < equipTools.Count; i++)
        {
            if (equipItemUI.itemSlots[i].isSelected)
            {
                if (context.phase == InputActionPhase.Started && !equipped)
                {
                    curTool = Instantiate(equipTools[i], toolPivot.transform);
                    equipped = true;
                }
                else if (context.phase == InputActionPhase.Started && equipped)
                {
                    Destroy(curTool);
                    toolAnimator = null;
                    equipped = false;
                }
            }
        }        
    }

    // Use InputAction
    public void OnUse(InputAction.CallbackContext context)
    {
        if (equipped && !nearWater && context.phase == InputActionPhase.Started)
        {
            playerAnimator.SetTrigger("Use");
            if (curTool.CompareTag("Axe") || curTool.CompareTag("Hoe")) toolAnimator.SetTrigger("Use");            
        }

        if (equipped && nearWater && context.phase == InputActionPhase.Started)
        {
            playerAnimator.SetTrigger("Use");
            if (curTool.CompareTag("Rod")) toolAnimator.SetBool("Fishing", true);
            else toolAnimator.SetTrigger("Use");
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
}
