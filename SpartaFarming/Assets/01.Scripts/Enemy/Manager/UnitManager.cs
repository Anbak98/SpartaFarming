using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public int hp;          // hp
    public float speed;     // �ӵ�
    public float searchRange;   // ��ġ����
    public float attackRange;   // ���� ���� 

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
    // �̱���
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


    // ���߿� csv �� ������ �� Dictionary<int, Unit> �̷��� ������ ��
    private Dictionary<int, Unit> numberToUnit;

    public Unit GetUnit(int num) { return numberToUnit[num]; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        numberToUnit = new Dictionary<int, Unit>();

        // ##TODO: �ӽ� ���� 
        Unit unit = new Unit(10, 3f, 5f, 2f);
        numberToUnit.Add(0,unit);
    }
}
