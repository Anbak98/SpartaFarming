using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;



public class PoolingEnemyGenerator : Singleton<PoolingEnemyGenerator>
{
    /// <summary>
    /// 오브젝트Pool 사용법
    /// 1. ObjectPool<T>을 생성
    /// T는 MonoBehaviour을 상속받은 타입이여야 한다.
    /// 2. IObjectFactory을 구현한 factory클래스가 있어야 한다 (이름 아무거나 상관x)
    /// 해당 클래스 내부에서 오브젝트를 인스턴스화 후 return, 필요한 값 할당 등의 작업수행 
    /// 
    /// ObjectPool의 생성자
    /// 1. 생성한 factory 
    /// 생성할 프리팹을 매개변수로 
    /// 
    /// 2. 오브젝트 가져올 때
    /// slimePool.GetPool();
    /// 
    /// 3. 오브젝트 넣을 때
    /// 해당 클래스의 ReturnToPool() 메서드 참고
    /// 
    /// </summary>
    /// 
    [SerializeField] GameObject orcPrefab;
    [SerializeField] GameObject slimePrefab;

    [SerializeField] Transform orcParent;
    [SerializeField] Transform slimeParent;
    
    [SerializeField] ObjectPool<PoolOrcEntity> orcPool;
    [SerializeField] ObjectPool<PoolSlimeEntity> slimePool;

    public void Start()
    {
        orcPool = new ObjectPool<PoolOrcEntity>(new EntitySlimeFactory(orcPrefab), 10 , orcParent);
        slimePool = new ObjectPool<PoolSlimeEntity> (new EntityOrcFactory(slimePrefab) , 10 , slimeParent);

        StartCoroutine(SlimeTest());
    }

    IEnumerator SlimeTest() 
    {
        yield return new WaitForSeconds(1);
        while (true) 
        {
            GameObject obj = slimePool.GetPool();
            yield return new WaitForSeconds(1.5f);
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
