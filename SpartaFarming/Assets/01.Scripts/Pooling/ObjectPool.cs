using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPool<T> where T : Component
{
    /// <summary>
    /// Ÿ�� T�� �������� ������Ʈ (Monobehavior �̿�����)
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

        // �ʱ�ȭ
        InitPool();
    }

    private void InitPool() 
    {
        if(pool == null)
            pool = new Queue<T>();

        for (int i = 0; i < initCount; i++) 
        {
            // ���丮���� �ν��Ͻ� ���� 
            T temp = InstanceT();
            pool.Enqueue(temp);
        }
    }

    private T InstanceT() 
    {
        // ����
        T temp = factory.CreateInstance();
        // �ʱ�ȭ, �θ� ���� 
        temp.transform.InitTransform( parent );
        // ����
        temp.gameObject.SetActive(false);

        return temp;
    }

    public GameObject GetPool() 
    {
        T getObject = null;

        // pool�� ������ 
        if (pool.Count <= 0)
        {
            // ����
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
