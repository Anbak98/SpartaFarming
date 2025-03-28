using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float curSpeed;
    public float moveSpeed;
    public float runSpeed;
    
    private Vector2 curMovementInput;    
    private float horizontal;
    private float vertical;

    private float plLastMoveX;
    private float plLastMoveY;

    private Rigidbody2D _rigidbody;
    private Animator playerAnimator;
    public Animator toolAnimator;
    public bool equipped = false;
    
    public GameObject curTool;
    public GameObject toolPivot;
    public GameObject harvestTool;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        
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
        curSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        Vector3 dir = transform.up * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= curSpeed;

        _rigidbody.velocity = dir;        
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
        if (context.phase == InputActionPhase.Started && !equipped)
        {            
            curTool = Instantiate(harvestTool, toolPivot.transform);            
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
        if (equipped && context.phase == InputActionPhase.Started)
        {
            playerAnimator.SetTrigger("Use");
            toolAnimator.SetTrigger("Use");
        }
    }
}
