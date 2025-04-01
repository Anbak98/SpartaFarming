using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public int hp;          // hp
    public float speed;     // 속도
    public float prowlRange;                // 배회 랜덤 범위 
    public float trackingTriggerRange;      // 서치범위 ( prowl -> tracking )
    public float attackTriggerRange;        // 공격 범위 ( tracking -> Attack )

    public float hitRange;                  // 히트 범위 
    public float attackDamage;              // 공격 데미지 
    public float attackCoolTime;            // 공격 쿨타임 

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

        // 슬라임 
        Unit slime = new Unit(10, 3f, 5f, 0, 0, 0, 0, 0);
        numberToUnit.Add(UnitType.Slime, slime);

        // 오크
        Unit orc = new Unit(10, 3f, 5f, 5f, 1f, 2f, 10, 3f);
        numberToUnit.Add(UnitType.Orc, orc);

        // 쎈 오크
        Unit strongOrc = new Unit(10, 3f, 5f, 5f, 1f, 2f, 10, 3f);
        numberToUnit.Add(UnitType.StrongOrc, strongOrc);

        // 묘비
        Unit graveStone = new Unit(10, 1f, 5f, 5f, 1f, 2f, 10, 3f);
        numberToUnit.Add(UnitType.GraveStone, graveStone);
    }
}
