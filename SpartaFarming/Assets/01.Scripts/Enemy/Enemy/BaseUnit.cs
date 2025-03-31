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

public class BaseUnit : MonoBehaviour
{
    [Header("===State===")]
    [SerializeField] private int unitNumber;
    [SerializeField] private Unit unitState;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("===Player===")]
    [SerializeField] private Transform player;

    // ##TODO : FSMHanlder로 분리 예정 
    // FSM
    [Header("===FSM===")]
    [SerializeField] private HeadMachine<BaseUnit> unitHeadMachine;
    [SerializeField] private FSM[] stateArray;
    [SerializeField] private EnemyState currState;
    [SerializeField] private EnemyState preState;

    public Transform Player { get => player; }
    public LayerMask ObstacleLayer { get => obstacleLayer; }
    public Unit UnitState { get => unitState; }

    private void Awake()
    {
        
    }

    private void Start() 
    {
        // FSM 초기화
        InitFSMArray();

        // ##TODO : unit클래스 초기화 
        unitNumber = 0;
        unitState = UnitManager.Instance.GetUnit(unitNumber);

        // UnitManager에서 초기화 
        player = UnitManager.Instance.PlayerTrs;
        obstacleLayer = UnitManager.Instance.ObstacleLayer;

        // 현재 상태 실행
        unitHeadMachine.HM_StateEnter();
    }

    private void OnEnable()
    {
        // 현재 상태 실행
        // unitHeadMachine.HM_StateEnter();
    }

    private void Update()
    {
        unitHeadMachine.HM_StateExcute();
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
        attackComponent.IAttackInit(this);
        trackingComponent.ITrakingInit(this);
        prowlComponent.IProwlInit(this);
        dieComponent.IDieInit(this);

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
    public void Flip() 
    {
        // 플레이어 기준 flip
        if (player.position.x > transform.position.x)
        {
            // 오른쪽 바라보기
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            // 왼쪽 바라보기
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

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

    public void IsDie() 
    {
        if (unitState.hp <= 0)
            ChageState(EnemyState.Die);
    }
}
