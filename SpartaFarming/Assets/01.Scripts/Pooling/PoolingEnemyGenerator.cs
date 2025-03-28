using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;



public class PoolingEnemyGenerator : Singleton<PoolingEnemyGenerator>
{
    [SerializeField] GameObject orcPrefab;
    [SerializeField] GameObject slimePrefab;
    [SerializeField] GameObject emptyObject;

    [SerializeField] Transform orcParent;
    [SerializeField] Transform slimeParent;
    [SerializeField] Transform emptyObjectParent;
    
    [SerializeField] ObjectPool<PoolOrcEntity> orcPool;
    [SerializeField] ObjectPool<PoolSlimeEntity> slimePool;

    // ���ӿ�����Ʈ�� Ǯ�� Transform���� ���� 
    [SerializeField] ObjectPool<Transform> emptyObjectPool;


    public void Start()
    {
        // T �� Monobehavior�� ��ӹ��� Ŭ������ ���
        orcPool = new ObjectPool<PoolOrcEntity>(new EntitySlimeFactory(orcPrefab), 10 , orcParent);
        slimePool = new ObjectPool<PoolSlimeEntity> (new EntityOrcFactory(slimePrefab) , 10 , slimeParent);

        // T �� ���ӿ�����Ʈ�϶�
        emptyObjectPool = new ObjectPool<Transform>(new EmptyFactory(emptyObject), 10, emptyObjectParent);

        // StartCoroutine(SlimeTest());
        StartCoroutine(EmptyTest());
    }

    // Empty Get �׽�Ʈ
    IEnumerator EmptyTest() 
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            GameObject obj = emptyObjectPool.GetPool();
            yield return new WaitForSeconds(1.5f);
        }
    }

    // ������ Get �׽�Ʈ 
    IEnumerator SlimeTest() 
    {
        yield return new WaitForSeconds(1);
        while (true) 
        {
            GameObject obj = orcPool.GetPool();
            yield return new WaitForSeconds(3f);
        }
    }

    public void ReturnToPool(GameObject obj) 
    {
        switch (obj.name) 
        {
            case "Orc(Clone)":
                orcPool.SetObject(obj);
                break;
            case "Slime(Clone)":
                slimePool.SetObject(obj);
                break;
        }
    }
}
