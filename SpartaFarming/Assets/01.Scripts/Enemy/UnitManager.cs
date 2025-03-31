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
}

public class UnitManager : Singleton<UnitManager>
{
    // ���߿� csv �� ������ �� Dictionary<int, Unit> �̷��� ������ ��
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

        // ##TODO: �ӽ� ���� 
        Unit unit = new Unit(10, 3f, 5f, 5f, 1f, 2f, 10 , 3f);
        numberToUnit.Add(0,unit);

        Unit slime = new Unit(10, 3f, 5f,0, 0,0,0,0);
        numberToUnit.Add(1, slime);
    }
}
