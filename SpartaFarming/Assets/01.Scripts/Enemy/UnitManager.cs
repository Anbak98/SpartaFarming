using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        // Hp , speed, 배회 범위 , tracking 범위 , attack 범위, 공격범위, 데미지 , 쿨타임

        // 슬라임 
        Unit slime = new Unit(10, 1f, 5f, 0, 0, 0, 0, 0);
        numberToUnit.Add(UnitType.Slime, slime);

        // 오크
        Unit orc = new Unit(10, 1.5f, 5f, 5f, 2f, 2f, 3, 3f);
        numberToUnit.Add(UnitType.Orc, orc);

        // 쎈 오크
        Unit strongOrc = new Unit(10, 1.5f, 3f, 2f, 2f, 2f, 10, 3f);
        numberToUnit.Add(UnitType.StrongOrc, strongOrc);

        // 묘비
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
