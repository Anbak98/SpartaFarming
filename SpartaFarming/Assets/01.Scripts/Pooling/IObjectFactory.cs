using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectFactory<T>
    where T : Component
{
    public T CreateInstance();
}

public class EmptyFactory : IObjectFactory<Transform>
{
    private GameObject empty;

    // 持失切
    public EmptyFactory(GameObject em)
    {
        this.empty = em;
    }

    public Transform CreateInstance()
    {
        GameObject orc = UnityEngine.Object.Instantiate(empty);
        return orc.GetComponent<Transform>();
    }
}

public class EntityOrcFactory : IObjectFactory<PoolSlimeEntity>
{
    private GameObject orcPrefab;

    // 持失切
    public EntityOrcFactory(GameObject orc)
    {
        this.orcPrefab = orc;
    }

    // 持失 板 return 
    public PoolSlimeEntity CreateInstance()
    {
        GameObject orc = UnityEngine.Object.Instantiate(orcPrefab);
        return orc.GetComponent<PoolSlimeEntity>();
    }

}

public class EntitySlimeFactory : IObjectFactory<PoolOrcEntity>
{
    private GameObject slimePrefab;

    // 持失切
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

