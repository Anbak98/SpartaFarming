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
    [SerializeField] private int nowFloor;      // 현재 층수

    [Header("===Container===")]
    [SerializeField]
    private Dictionary<int, DungeonEnemy> dungeonEnemies;
    [SerializeField]
    private Transform spanwPointParent;
    // ㄴ spanwPoint
    //      ㄴ Floor_ 1
    //          ㄴ point1
    //          ㄴ point2
    //          ㄴ point3
    //      ㄴ Floor_ 2

    [Header("===Action===")]
    private Action? enterFloor;

    [Header("===Script===")]
    public DungeonUI dungeonUi;

    private void Awake()
    {
        // 현재 층수
        nowFloor = 0;

        // 던전 데이터 초기화 
        InitDungeonEnemy();

        // 이벤트 추가
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

        // 층 별 던전데이터
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
        // 현재 층 +1
        nowFloor++;

        // 현재 층에 대한 데이터를 가지고 몬스터 생성
        if (dungeonEnemies.ContainsKey(nowFloor)) 
        {
            // 현재 몬스터 타입
            List<UnitType> currUnitType = dungeonEnemies[nowFloor].type;
            // 현재 자식 스폰포인트
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
        // 생성할 Unit 타입, 생성할 위치
        // UnitType이 여러개면 랜덤으로 생성

        if (trs == null)
            return;

        int ran;
        UnitType type;

        for (int i = 0; i < trs.childCount; i++)
        {
            ran = Random.Range(0, unitTypes.Count);
            type = unitTypes[ran];

            // type에 따른 오브젝트 return
            var obj = UnitManager.Instance.GetBaseUnit(type);
            obj.transform.position = trs.GetChild(i).transform.position;
        }
    }

}
