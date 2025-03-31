using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState 
{
    Idle,       // �⺻
    Prowl,      // ��ȸ
    Tracking,   // ����
    Attack,     // ����
    Die         // ���

}

public class BaseUnit : MonoBehaviour
{
    [Header("===State===")]
    [SerializeField] private int unitNumber;
    [SerializeField] private Unit unitState;

    // ##TODO : FSMHanlder�� �и� ���� 
    // FSM
    [Header("===FSM===")]
    [SerializeField] private HeadMachine<BaseUnit> unitHeadMachine;
    [SerializeField] private FSM[] stateArray;
    [SerializeField] private EnemyState currState;
    [SerializeField] private EnemyState preState;

    private void Awake()
    {
        // FSM �ʱ�ȭ
        InitFSMArray();

        unitNumber = 0;
        unitState = UnitManager.Instance.GetUnit(unitNumber);
    }

    private void Start() 
    {
        // ���� ���� ����
        unitHeadMachine.HM_StateEnter();
    }

    private void OnEnable()
    {
        // ���� ���� ����
        // unitHeadMachine.HM_StateEnter();
    }

    private void Update()
    {
        unitHeadMachine.HM_StateExcute();
    }


    private void InitFSMArray() 
    {
        // �ص�ӽ� �ʱ�ȭ 
        unitHeadMachine = new HeadMachine<BaseUnit>(this);

        stateArray = new FSM[Enum.GetValues(typeof(EnemyState)).Length];

        stateArray[(int)EnemyState.Prowl]       = new ProwlState<BaseUnit>(this, gameObject.GetComponent<IProwl>());
        stateArray[(int)EnemyState.Tracking]    = new TrackingState<BaseUnit>(this , gameObject.GetComponent<ITraking>());
        // stateArray[(int)EnemyState.Attack]      = new AttackState<BaseUnit>(this, gameObject.GetComponent<IAttack>());
        stateArray[(int)EnemyState.Die]         = new DieState<BaseUnit>(this , gameObject.GetComponent<IDie>());

        // ##TODO : �ӽ� ��ȸ ���� 
        currState = EnemyState.Prowl;

        // ���� ���� �ʱ�ȭ 
        unitHeadMachine.HM_InitMachine(stateArray[(int)currState]);
    }

}
