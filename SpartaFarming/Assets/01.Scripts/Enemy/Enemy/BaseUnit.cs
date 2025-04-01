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

    // ## TODO : handler�� �ٲٱ�
    [Header("===FSM===")]
    [SerializeField] private HeadMachine<BaseUnit> unitHeadMachine;
    [SerializeField] private FSM[] stateArray;
    [SerializeField] private EnemyState currState;
    [SerializeField] private EnemyState preState;

    // ������Ƽ
    public Transform Player { get => player; }
    public Unit UnitState { get => unitState; }
    public UnitType UnitType { get => unitType; }

    private void Start() 
    {
        // �ڵ鷯 �ʱ�ȭ
        animaionHandler = new EnemyAnimationHandler(GetComponent<Animator>());

        // FSM �ʱ�ȭ
        InitFSMArray();

        // UnitManager���� �ʱ�ȭ 
        player          = UnitManager.Instance.PlayerTrs;

        // ���� ���� ����
        unitHeadMachine.HM_StateEnter();
    }

    private void OnEnable()
    {
        Debug.Log("���� Enable");

        // ���� �ʱ�ȭ 
        currState = EnemyState.Prowl;
        unitHeadMachine.HM_InitMachine(StateToFSM(currState));

        // ���� ���� ����
        unitHeadMachine.HM_StateEnter();
    }

    private void Update()
    {
        unitHeadMachine.HM_StateExcute();
    }

    // pool���� ���� �� �ʱ�ȭ 
    public void UnitStateInit(UnitType type ,Unit unit) 
    {
        this.unitType = type;
        this.unitState = unit;

        oriHp = unit.hp;
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
        if(attackComponent != null) attackComponent.IAttackInit(this);
        if(trackingComponent != null) trackingComponent.ITrakingInit(this);
        if(prowlComponent != null) prowlComponent.IProwlInit(this);
        if(dieComponent != null) dieComponent.IDieInit(this);

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
        if (StateToFSM(nextState) == null) 
            return;
        
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
    public void Flip( float standardX  ) 
    {
        // �Ű������ε��� x ���� flip
        if (standardX > transform.position.x)
        {
            // ������ �ٶ󺸱�
            transform.localEulerAngles = new Vector3(0,0,0);
        }
        else
        {
            // ���� �ٶ󺸱�
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }

    // �����ȿ� �ִ��� ?
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

    // ������ -> ���º�ȭ 
    public void IsDie() 
    {
        if (unitState.hp <= 0)
            ChageState(EnemyState.Die);
    }

    // �ִϸ��̼� ����
    public void ChangeAnimation( EnemyAnimationState state ) 
    {
        animaionHandler.ChangeAnimator(state);
    }

    // Die �� �ʱ�ȭ
    public void InitDie() 
    {
        // 1. Hp �������
        UnitState.hp = oriHp;
    }

    // �浹 ���� 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == UnitManager.Instance.PlayerLayerInt) 
        {
            // �ӽ� : 1�� ����
            unitState.hp -= 1;
        }
    }
}
