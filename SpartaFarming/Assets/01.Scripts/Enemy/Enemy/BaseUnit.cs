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

        // ##TODO : unitŬ���� �ʱ�ȭ 
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

        // �迭 �ʱ�ȭ 
        stateArray = new FSM[Enum.GetValues(typeof(EnemyState)).Length];

        // �������̽� �������� 
        IAttack<Unit> attackComponent       = GetComponent<IAttack<Unit>>();
        ITraking<Unit> trackingComponent    = GetComponent<ITraking<Unit>>();
        IProwl<Unit> prowlComponent         = GetComponent<IProwl<Unit>>();
        IDie<Unit> dieComponent             = GetComponent<IDie<Unit>>();

        // unit ������ �־��ֱ�
        attackComponent.IAttackInit(unitState);
        trackingComponent.ITrakingInit(unitState);
        prowlComponent.IProwlInit(unitState);
        dieComponent.IDieInit(unitState);

        // FSM �迭 �ʱ�ȭ 
        stateArray[(int)EnemyState.Attack]      = new AttackState<BaseUnit,Unit>(this, attackComponent);
        stateArray[(int)EnemyState.Tracking]    = new TrackingState<BaseUnit , Unit>(this , trackingComponent);
        stateArray[(int)EnemyState.Prowl]       = new ProwlState<BaseUnit, Unit>(this, prowlComponent);
        stateArray[(int)EnemyState.Die]         = new DieState<BaseUnit , Unit>(this , dieComponent);

        // ##TODO : �ӽ� ��ȸ ���� 
        currState = EnemyState.Prowl;

        // ���� ���� �ʱ�ȭ 
        unitHeadMachine.HM_InitMachine(stateArray[(int)currState]);
    }

}
