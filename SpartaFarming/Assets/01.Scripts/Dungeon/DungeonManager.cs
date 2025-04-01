using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class DungeonEnemy 
{
    public int floor;
    public List<UnitType> type;

    public DungeonEnemy(int floor ,List<UnitType> types) 
    { 
        this.floor = floor;
        this.type = types;
    }
}

public class DungeonManager : Singleton<DungeonManager>
{
    [Header("===FLOOR===")]
    [SerializeField] private int nowFloor;      // ���� ����

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<int, DungeonEnemy> dungeonEnemies;
    [SerializeField]
    private Transform spanwPointParent;
    // �� spanwPoint
    //      �� Floor_ 1
    //          �� point1
    //          �� point2
    //          �� point3
    //      �� Floor_ 2

    [Header("===Action===")]
    private Action? enterFloor;

    [Header("===Script===")]
    public DungeonUI dungeonUi;

    private void Awake()
    {
        // ���� ����
        nowFloor = 0;

        // ���� ������ �ʱ�ȭ 
        InitDungeonEnemy();

        // �̺�Ʈ �߰�
        this.AddEvent(EnterDungeon);
    }

    private void Start()
    {
        EventInvoke();
    }

    private void InitDungeonEnemy() 
    {
        dungeonEnemies = new Dictionary<int, DungeonEnemy> ();

        // ##TODO : Try using the data table
        DungeonEnemy enemyGroup1 = new DungeonEnemy( 1,
            new List<UnitType> { UnitType.Slime, UnitType.GraveStone });
        DungeonEnemy enemyGroup2 = new DungeonEnemy( 2,
            new List<UnitType> { UnitType.Orc, UnitType.StrongOrc });

        // �� �� ����������
        dungeonEnemies.Add(enemyGroup1.floor , enemyGroup1);
        dungeonEnemies.Add(enemyGroup2.floor , enemyGroup2);
    }

    public void AddEvent(Action myAction) 
    {
        enterFloor += myAction;
    }

    public void EventInvoke() 
    {
        enterFloor?.Invoke();
    }

    public void EnterDungeon()
    {
        // ���� �� +1
        nowFloor++;

        // ���� ���� ���� �����͸� ������ ���� ����
        if (dungeonEnemies.ContainsKey(nowFloor)) 
        {
            // ���� ���� Ÿ��
            List<UnitType> currUnitType = dungeonEnemies[nowFloor].type;
            // ���� �ڽ� ��������Ʈ
            Transform currSpanPoint;
            try 
            {
                currSpanPoint = spanwPointParent.GetChild(nowFloor - 1);
            }
            catch (Exception ex) { currSpanPoint = null; }

            GenerateEnemy(currUnitType , currSpanPoint);
        }
    }

    public void GenerateEnemy(List<UnitType> unitTypes, Transform trs)
    {
        // ������ Unit Ÿ��, ������ ��ġ
        // UnitType�� �������� �������� ����

        if (trs == null)
            return;

        int ran;
        UnitType type;

        for (int i = 0; i < trs.childCount; i++)
        {
            ran = Random.Range(0, unitTypes.Count);
            type = unitTypes[ran];

            // type�� ���� ������Ʈ return
            var obj = UnitManager.Instance.GetBaseUnit(type);
            obj.transform.position = trs.GetChild(i).transform.position;
        }
    }

}
