using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    [Header("===POOL===")]
    [SerializeField]
    private ObjectPool<BaseUnit> slimePool;
    [SerializeField]
    private ObjectPool<BaseUnit> orcPool;
    [SerializeField]
    private ObjectPool<BaseUnit> strongOrcPool;
    [SerializeField]
    private ObjectPool<BaseUnit> gravePool;

    [Header("===Prefab===")]
    [SerializeField] GameObject[] slimePrefab;
    [SerializeField] GameObject[] orcPrefab;
    [SerializeField] GameObject[] strongOrcPrefab;
    [SerializeField] GameObject[] graveStonePrefab;

    [Header("===Parnet===")]
    [SerializeField] Transform slimeParent;
    [SerializeField] Transform oreParent;
    [SerializeField] Transform strongOrcParent;
    [SerializeField] Transform graveStoneParent;

    [Header("===GeneratMonster===")]
    [SerializeField] List<GameObject> monsterGene;

    public Transform PlayerTrs { get => playerTrs; }
    public LayerMask ObstacleLayer { get => obstacleLayer;}
    public LayerMask PlayerLayer { get => playerLayer; }

    public Unit GetUnit(UnitType type) { return numberToUnit[type]; }

    private void Awake()
    {
        InitEnemyState();

        slimePool = new ObjectPool<BaseUnit>(new EnemyFactory(slimePrefab, UnitType.Slime), 3, slimeParent);
        orcPool = new ObjectPool<BaseUnit>(new EnemyFactory(orcPrefab, UnitType.Orc), 3, oreParent);
        strongOrcPool = new ObjectPool<BaseUnit>(new EnemyFactory(strongOrcPrefab, UnitType.StrongOrc), 3, strongOrcParent);
        gravePool = new ObjectPool<BaseUnit>(new EnemyFactory(graveStonePrefab, UnitType.GraveStone), 3, graveStoneParent);
    }

    private void Start()
    {
        StartCoroutine(Generate());
    }

    private void InitEnemyState() 
    {
        numberToUnit = new Dictionary<UnitType, Unit>();
        // Hp , speed, ��ȸ ���� , tracking ���� , attack ����, ���ݹ���, ������ , ��Ÿ��

        // ������ 
        Unit slime = new Unit(10, 1f, 5f, 0, 0, 0, 0, 0);
        numberToUnit.Add(UnitType.Slime, slime);

        // ��ũ
        Unit orc = new Unit(10, 1.5f, 5f, 5f, 2f, 2f, 3, 3f);
        numberToUnit.Add(UnitType.Orc, orc);

        // �� ��ũ
        Unit strongOrc = new Unit(10, 1.5f, 3f, 2f, 2f, 2f, 10, 3f);
        numberToUnit.Add(UnitType.StrongOrc, strongOrc);

        // ����
        Unit graveStone = new Unit(10, 0.5f, 2f, 5f, 2f, 2f, 6, 5f);
        numberToUnit.Add(UnitType.GraveStone, graveStone);
    }

    IEnumerator Generate() 
    {
        for(int i = 0; i < 12; i++) 
        {
            if (i % 3 == 0)
                yield return new WaitForSeconds(3f);

            var temp = orcPool.GetPool();
            temp.transform.position = new Vector2(10,10);

            monsterGene.Add(temp);
            yield return new WaitForSeconds(1f);
        }

    }

    public void ReturnToPool(UnitType state, GameObject obj) 
    {
        switch (state) 
        {
            case UnitType.Slime:
                slimePool.SetObject(obj); break;
            case UnitType.Orc:
                orcPool.SetObject(obj); break;
            case UnitType.StrongOrc:
                strongOrcPool.SetObject(obj); break;
            case UnitType.GraveStone:
                gravePool.SetObject(obj); break;

        }
    }
    
}
