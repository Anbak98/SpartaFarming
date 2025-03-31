using System;
using System.Collections;
using System.Collections.Generic;
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

    // ##TODO : FSMHanlder로 분리 예정 
    // FSM
    [Header("===FSM===")]
    [SerializeField] private HeadMachine<BaseUnit> unitHeadMachine;
    [SerializeField] private FSM[] stateArray;
    [SerializeField] private EnemyState currState;
    [SerializeField] private EnemyState preState;

    private void Awake()
    {
        // FSM 초기화
        InitFSMArray();

        // ##TODO : unit클래스 초기화 
        unitNumber = 0;
        unitState = UnitManager.Instance.GetUnit(unitNumber);
    }

    private void Start() 
    {
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
        IAttack<Unit> attackComponent       = GetComponent<IAttack<Unit>>();
        ITraking<Unit> trackingComponent    = GetComponent<ITraking<Unit>>();
        IProwl<Unit> prowlComponent         = GetComponent<IProwl<Unit>>();
        IDie<Unit> dieComponent             = GetComponent<IDie<Unit>>();

        // unit 데이터 넣어주기
        attackComponent.IAttackInit(unitState);
        trackingComponent.ITrakingInit(unitState);
        prowlComponent.IProwlInit(unitState);
        dieComponent.IDieInit(unitState);

        // FSM 배열 초기화 
        stateArray[(int)EnemyState.Attack]      = new AttackState<BaseUnit,Unit>(this, attackComponent);
        stateArray[(int)EnemyState.Tracking]    = new TrackingState<BaseUnit , Unit>(this , trackingComponent);
        stateArray[(int)EnemyState.Prowl]       = new ProwlState<BaseUnit, Unit>(this, prowlComponent);
        stateArray[(int)EnemyState.Die]         = new DieState<BaseUnit , Unit>(this , dieComponent);

        // ##TODO : 임시 배회 상태 
        currState = EnemyState.Prowl;

        // 현재 상태 초기화 
        unitHeadMachine.HM_InitMachine(stateArray[(int)currState]);
    }

}
