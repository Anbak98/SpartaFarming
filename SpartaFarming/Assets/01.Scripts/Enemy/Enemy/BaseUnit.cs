using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private LayerMask obstacleLayer;

    [Header("===Player===")]
    [SerializeField] private Transform player;

    // ##TODO : FSMHanlder�� �и� ���� 
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
        // FSM �ʱ�ȭ
        InitFSMArray();

        // ##TODO : unitŬ���� �ʱ�ȭ 
        unitNumber = 0;
        unitState = UnitManager.Instance.GetUnit(unitNumber);

        // UnitManager���� �ʱ�ȭ 
        player = UnitManager.Instance.PlayerTrs;
        obstacleLayer = UnitManager.Instance.ObstacleLayer;

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
        IAttack<BaseUnit> attackComponent       = GetComponent<IAttack<BaseUnit>>();
        ITraking<BaseUnit> trackingComponent    = GetComponent<ITraking<BaseUnit>>();
        IProwl<BaseUnit> prowlComponent         = GetComponent<IProwl<BaseUnit>>();
        IDie<BaseUnit> dieComponent             = GetComponent<IDie<BaseUnit>>();

        // unit ������ �־��ֱ�
        attackComponent.IAttackInit(this);
        trackingComponent.ITrakingInit(this);
        prowlComponent.IProwlInit(this);
        dieComponent.IDieInit(this);

        // FSM �迭 �ʱ�ȭ 
        stateArray[(int)EnemyState.Attack]      = new AttackState<BaseUnit>(this, attackComponent);
        stateArray[(int)EnemyState.Tracking]    = new TrackingState<BaseUnit>(this , trackingComponent);
        stateArray[(int)EnemyState.Prowl]       = new ProwlState<BaseUnit>(this, prowlComponent);
        stateArray[(int)EnemyState.Die]         = new DieState<BaseUnit>(this , dieComponent);

        // ##TODO : �ӽ� ��ȸ ���� 
        currState = EnemyState.Prowl;

        // ���� ���� �ʱ�ȭ 
        unitHeadMachine.HM_InitMachine(StateToFSM(currState));
    }

    public void ChageState(EnemyState nextState) 
    {
        // �ð��� 
        preState = currState;
        currState = nextState;

        // ���ӽſ� �ֱ� 
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
        // prowl -> Tracking ���� 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position , unitState.trackingTriggerRange);

        // Tracking -> Attack ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitState.attackTriggerRange);
    }

    // �÷��̾� ���� �ø�
    public void Flip() 
    {
        // �÷��̾� ���� flip
        if (player.position.x > transform.position.x)
        {
            // ������ �ٶ󺸱�
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            // ���� �ٶ󺸱�
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public bool isInRange(float standardDistance) 
    {
        // �� ��ġ , �÷��̾� ��ġ �Ÿ� 
        // ���� �Ÿ� �ȿ� ������ true
        float dis = Vector2.Distance(transform.position , player.position);
        if (dis <= standardDistance)
            return true;
        // ������ false
        else 
            return false;
    }

    public void IsDie() 
    {
        if (unitState.hp <= 0)
            ChageState(EnemyState.Die);
    }
}
