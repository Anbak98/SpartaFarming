using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPool<T> where T : Component
{
    /// <summary>
    /// 타입 T는 실질적인 오브젝트 (Monobehavior 이여야함)
    /// </summary>

    [Header("=== Container ===")]
    private Queue<T> pool = new Queue<T>();
    [Header("=== Factory ===")]
    private IObjectFactory<T> factory;
    [Header("=== State ===")]
    private Transform parent;
    private int initCount;

    public ObjectPool(IObjectFactory<T> factory, int initCnt , Transform parentTrs = null) 
    { 
        this.factory = factory;
        this.initCount = initCnt;
        if (parentTrs != null)
            this.parent = parentTrs;

        // 초기화
        InitPool();
    }

    private void InitPool() 
    {
        if(pool == null)
            pool = new Queue<T>();

        for (int i = 0; i < initCount; i++) 
        {
            // 펙토리에서 인스턴스 생성 
            T temp = InstanceT();
            pool.Enqueue(temp);
        }
    }

    private T InstanceT() 
    {
        // 생성
        T temp = factory.CreateInstance();
        // 초기화, 부모 설정 
        temp.transform.InitTransform( parent );
        // 끄기
        temp.gameObject.SetActive(false);

        return temp;
    }

    public GameObject GetPool() 
    {
        T getObject = null;

        // pool에 없으면 
        if (pool.Count <= 0)
        {
            // 생성
            getObject = InstanceT();
            pool.Enqueue(getObject);
        }

        getObject = pool.Dequeue();
        getObject.gameObject.SetActive(true);

        return getObject.gameObject;
    }

    public void SetObject(GameObject obj) 
    {
        obj.SetActive(false);
        pool.Enqueue(obj.GetComponent<T>());
    }
}
