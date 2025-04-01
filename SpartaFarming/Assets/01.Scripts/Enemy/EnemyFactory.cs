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
        // �������� ������Ʈ return
        int ran = Random.Range(0, slimePrefab.Length);

        // type�� ���� UnitŬ���� ��
        Unit stats = new Unit(new Unit(UnitManager.Instance.GetUnit(type)));

        // �ν��Ͻ�ȭ
        var slime = UnityEngine.GameObject.Instantiate(slimePrefab[ran]);
        var component = slime.GetComponent<BaseUnit>();
        component.UnitStateInit(type, stats);

        return component;
    }
}
