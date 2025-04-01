using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public int hp;          // hp
    public float speed;     // �ӵ�
    public float prowlRange;                // ��ȸ ���� ���� 
    public float trackingTriggerRange;      // ��ġ���� ( prowl -> tracking )
    public float attackTriggerRange;        // ���� ���� ( tracking -> Attack )

    public float hitRange;                  // ��Ʈ ���� 
    public float attackDamage;              // ���� ������ 
    public float attackCoolTime;            // ���� ��Ÿ�� 

    public Unit(int hp, float speed, float prowlRange,float searchRange, float attackRange,float hitRange ,float damage, float cooltime)
    {
        this.hp                     = hp;
        this.speed                  = speed;
        this.prowlRange             = prowlRange;  
        this.trackingTriggerRange   = searchRange;
        this.attackTriggerRange     = attackRange;
        this.hitRange               = hitRange;
        this.attackDamage = damage;
        this.attackCoolTime = cooltime;
    }

    public Unit(Unit unit) 
    {
        this.hp = unit.hp;
        this.speed = unit.speed;
        this.prowlRange = unit.prowlRange;
        this.trackingTriggerRange = unit.trackingTriggerRange;
        this.attackTriggerRange = unit.attackTriggerRange;
        this.hitRange = unit.hitRange;
        this.attackDamage = unit.attackDamage;
        this.attackCoolTime = unit.attackCoolTime;
    }
}

public enum UnitType
{ 
    Slime,
    Orc,
    StrongOrc,
    GraveStone
}

public class UnitManager : Singleton<UnitManager>
{
    private Dictionary<UnitType, Unit> numberToUnit;

    [SerializeField]
    private Transform playerTrs;
    [SerializeField]
    private LayerMask obstacleLayer;
    [SerializeField]
    private LayerMask playerLayer;

    public Transform PlayerTrs { get => playerTrs; }
    public LayerMask ObstacleLayer { get => obstacleLayer;}
    public LayerMask PlayerLayer { get => playerLayer; }

    public Unit GetUnit(UnitType type) { return numberToUnit[type]; }

    private void Awake()
    {
        InitEnemyState();
    }

    private void InitEnemyState() 
    {
        numberToUnit = new Dictionary<UnitType, Unit>();

        // ������ 
        Unit slime = new Unit(10, 3f, 5f, 0, 0, 0, 0, 0);
        numberToUnit.Add(UnitType.Slime, slime);

        // ��ũ
        Unit orc = new Unit(10, 3f, 5f, 5f, 1f, 2f, 10, 3f);
        numberToUnit.Add(UnitType.Orc, orc);

        // �� ��ũ
        Unit strongOrc = new Unit(10, 3f, 5f, 5f, 1f, 2f, 10, 3f);
        numberToUnit.Add(UnitType.StrongOrc, strongOrc);

        // ����
        Unit graveStone = new Unit(10, 1f, 5f, 5f, 1f, 2f, 10, 3f);
        numberToUnit.Add(UnitType.GraveStone, graveStone);
    }
}
