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
}

public class UnitManager : Singleton<UnitManager>
{
    // 나중에 csv 로 가져올 때 Dictionary<int, Unit> 이렇게 관리할 듯
    private Dictionary<int, Unit> numberToUnit;

    [SerializeField]
    private Transform playerTrs;
    [SerializeField]
    private LayerMask obstacleLayer;
    [SerializeField]
    private LayerMask playerLayer;

    public Transform PlayerTrs { get => playerTrs; }
    public LayerMask ObstacleLayer { get => obstacleLayer;}
    public LayerMask PlayerLayer { get => playerLayer; }

    public Unit GetUnit(int num) { return numberToUnit[num]; }

    private void Awake()
    {
        numberToUnit = new Dictionary<int, Unit>();

        // ##TODO: 임시 생성 
        Unit unit = new Unit(10, 3f, 5f, 5f, 1f, 2f, 10 , 3f);
        numberToUnit.Add(0,unit);

        Unit slime = new Unit(10, 3f, 5f,0, 0,0,0,0);
        numberToUnit.Add(1, slime);
    }
}
