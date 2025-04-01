using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyFactory : IObjectFactory<BaseUnit>
{
    private GameObject[] slimePrefab;
    private UnitType type;

    public EnemyFactory(GameObject[] prefab, UnitType type) 
    { 
        this.slimePrefab = prefab;
        this.type = type;
    }

    public BaseUnit CreateInstance()
    {
        // 랜덤으로 오브젝트 return
        int ran = Random.Range(0, slimePrefab.Length);

        // type에 따른 Unit클래스 깊복
        Unit stats = new Unit(new Unit(UnitManager.Instance.GetUnit(type)));

        // 인스턴스화
        var slime = UnityEngine.GameObject.Instantiate(slimePrefab[ran]);
        var component = slime.GetComponent<BaseUnit>();
        component.UnitStateInit(type, stats);

        return component;
    }
}
