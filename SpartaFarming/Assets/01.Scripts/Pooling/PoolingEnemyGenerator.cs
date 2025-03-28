using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;



public class PoolingEnemyGenerator : Singleton<PoolingEnemyGenerator>
{
    /// <summary>
    /// ������ƮPool ����
    /// 1. ObjectPool<T>�� ����
    /// T�� MonoBehaviour�� ��ӹ��� Ÿ���̿��� �Ѵ�.
    /// 2. IObjectFactory�� ������ factoryŬ������ �־�� �Ѵ� (�̸� �ƹ��ų� ���x)
    /// �ش� Ŭ���� ���ο��� ������Ʈ�� �ν��Ͻ�ȭ �� return, �ʿ��� �� �Ҵ� ���� �۾����� 
    /// 
    /// ObjectPool�� ������
    /// 1. ������ factory 
    /// ������ �������� �Ű������� 
    /// 
    /// 2. ������Ʈ ������ ��
    /// slimePool.GetPool();
    /// 
    /// 3. ������Ʈ ���� ��
    /// �ش� Ŭ������ ReturnToPool() �޼��� ����
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
