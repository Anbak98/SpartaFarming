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

        stateArray = new FSM[Enum.GetValues(typeof(EnemyState)).Length];

        stateArray[(int)EnemyState.Prowl]       = new ProwlState<BaseUnit>(this, gameObject.GetComponent<IProwl>());
        stateArray[(int)EnemyState.Tracking]    = new TrackingState<BaseUnit>(this , gameObject.GetComponent<ITraking>());
        // stateArray[(int)EnemyState.Attack]      = new AttackState<BaseUnit>(this, gameObject.GetComponent<IAttack>());
        stateArray[(int)EnemyState.Die]         = new DieState<BaseUnit>(this , gameObject.GetComponent<IDie>());

        // ##TODO : 임시 배회 상태 
        currState = EnemyState.Prowl;

        // 현재 상태 초기화 
        unitHeadMachine.HM_InitMachine(stateArray[(int)currState]);
    }

}
