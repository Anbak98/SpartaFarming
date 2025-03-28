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

    private Rigidbody2D _rigidbody;
    private Animator animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
   
    void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Move();
        animator.speed = curSpeed / moveSpeed;

        SetMoveRotAnime();
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
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        if (horizontal == 1 || horizontal == -1 || vertical == 1 || vertical == -1)
        {
            animator.SetFloat("LastMoveX", horizontal);
            animator.SetFloat("LastMoveY", vertical);
        }
    }
}
