using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public int hp;          // hp
    public float speed;     // �ӵ�
    public float trackingTriggerRange;      // ��ġ���� ( prowl -> tracking )
    public float attackTriggerRange;        // ���� ���� ( tracking -> Attack )
    public float hitRange;                  // ��Ʈ ���� 
    public float attackDamage;              // ���� ������ 

    public Unit(int hp, float speed, float searchRange, float attackRange, float hitRange , float damage)
    {
        this.hp                     = hp;
        this.speed                  = speed;
        this.trackingTriggerRange   = searchRange;
        this.attackTriggerRange     = attackRange;
        this.hitRange               = hitRange;
        this.attackDamage = damage;
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
        Unit unit = new Unit(10, 3f, 5f, 1f, 2f, 10);
        numberToUnit.Add(0,unit);
    }
}
