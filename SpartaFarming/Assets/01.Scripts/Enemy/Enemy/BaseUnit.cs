using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState 
{
    Idle,       // 기본
    Prowl,      // 배회
    Tracking,   // 추적
    Attack,     // 공격
    Die         // 사망

}

public enum EnemyAnimationState 
{
    Idle,
    Run,
    Attack,
    Hit,
    Die
}

public class BaseUnit : MonoBehaviour
{
    [Header("===State===")]
    [SerializeField] private UnitType unitType;
    [SerializeField] private Unit unitState;
    [SerializeField] private int oriHp;
    
    [Header("===Player===")]
    [SerializeField] private Transform player;

    [Header("===Handler===")]
    [SerializeField] EnemyAnimationHandler animaionHandler;

    // ## TODO : handler로 바꾸기
    [Header("===FSM===")]
    [SerializeField] private HeadMachine<BaseUnit> unitHeadMachine;
    [SerializeField] private FSM[] stateArray;
    [SerializeField] private EnemyState currState;
    [SerializeField] private EnemyState preState;

    // 프로퍼티
    public Transform Player { get => player; }
    public Unit UnitState { get => unitState; }
    public UnitType UnitType { get => unitType; }

    private void Start() 
    {
        // 핸들러 초기화
        animaionHandler = new EnemyAnimationHandler(GetComponent<Animator>());

        // FSM 초기화
        InitFSMArray();

        // UnitManager에서 초기화 
        player          = UnitManager.Instance.PlayerTrs;

        // 현재 상태 실행
        unitHeadMachine.HM_StateEnter();
    }

    private void OnEnable()
    {
        Debug.Log("몬스터 Enable");

        // 상태 초기화 
        currState = EnemyState.Prowl;
        unitHeadMachine.HM_InitMachine(StateToFSM(currState));

        // 현재 상태 실행
        unitHeadMachine.HM_StateEnter();
    }

    private void Update()
    {
        unitHeadMachine.HM_StateExcute();
    }

    // pool에서 생성 시 초기화 
    public void UnitStateInit(UnitType type ,Unit unit) 
    {
        this.unitType = type;
        this.unitState = unit;

        oriHp = unit.hp;
    }

    private void InitFSMArray() 
    {
        // 해드머신 초기화 
        unitHeadMachine = new HeadMachine<BaseUnit>(this);

        // 배열 초기화 
        stateArray = new FSM[Enum.GetValues(typeof(EnemyState)).Length];

        // 인터페이스 가져오기 
        IAttack<BaseUnit> attackComponent       = GetComponent<IAttack<BaseUnit>>();
        ITraking<BaseUnit> trackingComponent    = GetComponent<ITraking<BaseUnit>>();
        IProwl<BaseUnit> prowlComponent         = GetComponent<IProwl<BaseUnit>>();
        IDie<BaseUnit> dieComponent             = GetComponent<IDie<BaseUnit>>();

        // unit 데이터 넣어주기
        if(attackComponent != null) attackComponent.IAttackInit(this);
        if(trackingComponent != null) trackingComponent.ITrakingInit(this);
        if(prowlComponent != null) prowlComponent.IProwlInit(this);
        if(dieComponent != null) dieComponent.IDieInit(this);

        // FSM 배열 초기화 
        stateArray[(int)EnemyState.Attack]      = new AttackState<BaseUnit>(this, attackComponent);
        stateArray[(int)EnemyState.Tracking]    = new TrackingState<BaseUnit>(this , trackingComponent);
        stateArray[(int)EnemyState.Prowl]       = new ProwlState<BaseUnit>(this, prowlComponent);
        stateArray[(int)EnemyState.Die]         = new DieState<BaseUnit>(this , dieComponent);

        // ##TODO : 임시 배회 상태 
        currState = EnemyState.Prowl;

        // 현재 상태 초기화 
        unitHeadMachine.HM_InitMachine(StateToFSM(currState));
    }

    public void ChageState(EnemyState nextState) 
    {
        if (StateToFSM(nextState) == null) 
            return;
        
        // 시각용 
        preState = currState;
        currState = nextState;

        // 헤드머신에 넣기 
        unitHeadMachine.HM_ChangeState(StateToFSM(nextState));
    }

    private FSM StateToFSM(EnemyState staste) 
    {
        try 
        {
            return stateArray[(int)staste];
        }
        catch { return null; }
    }

    // Draw
    private void OnDrawGizmos()
    {
        // prowl -> Tracking 범위 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position , unitState.trackingTriggerRange);

        // Tracking -> Attack 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitState.attackTriggerRange);
    }

    // 플레이어 기준 플립
    public void Flip( float standardX  ) 
    {
        // 매개변수로들어온 x 기준 flip
        if (standardX > transform.position.x)
        {
            // 오른쪽 바라보기
            transform.localEulerAngles = new Vector3(0,0,0);
        }
        else
        {
            // 왼쪽 바라보기
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }

    // 범위안에 있는지 ?
    public bool isInRange(float standardDistance) 
    {
        // 내 위치 , 플레이어 위치 거리 
        // 기준 거리 안에 있으면 true
        float dis = Vector2.Distance(transform.position , player.position);
        if (dis <= standardDistance)
            return true;
        // 없으면 false
        else 
            return false;
    }

    // 죽으면 -> 상태변화 
    public void IsDie() 
    {
        if (unitState.hp <= 0)
            ChageState(EnemyState.Die);
    }

    // 애니메이션 변경
    public void ChangeAnimation( EnemyAnimationState state ) 
    {
        animaionHandler.ChangeAnimator(state);
    }

    // Die 시 초기화
    public void InitDie() 
    {
        // 1. Hp 원래대로
        UnitState.hp = oriHp;
    }

    // 충돌 감지 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == UnitManager.Instance.PlayerLayerInt) 
        {
            // 임시 : 1씩 감소
            unitState.hp -= 1;
        }
    }
}
