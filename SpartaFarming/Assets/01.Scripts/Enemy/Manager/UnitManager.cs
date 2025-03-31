using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public int hp;          // hp
    public float speed;     // 속도
    public float searchRange;   // 서치범위
    public float attackRange;   // 공격 범위 

    public Unit(int hp, float speed, float searchRange, float attackRange)
    {
        this.hp = hp;
        this.speed = speed;
        this.searchRange = searchRange;
        this.attackRange = attackRange;
    }
}

public class UnitManager : MonoBehaviour
{
    // 싱글톤
    private static UnitManager instance;
    public static UnitManager Instance 
    {
        get 
        {
            if (instance == null)
            {
                GameObject temp = new GameObject("UnitManager");
                instance = temp.GetComponent<UnitManager>();
                return instance;
            }
            return instance;
        }
    }


    // 나중에 csv 로 가져올 때 Dictionary<int, Unit> 이렇게 관리할 듯
    private Dictionary<int, Unit> numberToUnit;

    public Unit GetUnit(int num) { return numberToUnit[num]; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        numberToUnit = new Dictionary<int, Unit>();

        // ##TODO: 임시 생성 
        Unit unit = new Unit(10, 3f, 5f, 2f);
        numberToUnit.Add(0,unit);
    }
}
