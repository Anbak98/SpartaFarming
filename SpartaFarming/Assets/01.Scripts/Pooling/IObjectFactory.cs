using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectFactory<T>
    where T : class
{
    public T CreateInstance();
}

public class EntityOrcFactory : IObjectFactory<PoolSlimeEntity>
{
    private GameObject orcPrefab;

    // ������
    public EntityOrcFactory(GameObject orc)
    {
        this.orcPrefab = orc;
    }

    // ���� �� return 
    public PoolSlimeEntity CreateInstance()
    {
        GameObject orc = UnityEngine.Object.Instantiate(orcPrefab);
        return orc.GetComponent<PoolSlimeEntity>();
    }

}

public class EntitySlimeFactory : IObjectFactory<PoolOrcEntity>
{
    private GameObject slimePrefab;

    // ������
    public EntitySlimeFactory(GameObject sl)
    {
        this.slimePrefab = sl;
    }

    public PoolOrcEntity CreateInstance()
    {
        GameObject slime = UnityEngine.Object.Instantiate(slimePrefab);
        return slime.GetComponent<PoolOrcEntity>();
    }
}

