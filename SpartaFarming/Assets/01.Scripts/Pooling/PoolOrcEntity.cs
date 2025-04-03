using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolOrcEntity : PoolingEntity
{

    private void OnEnable()
    {
        StartCoroutine(returnOrc());
    }

    IEnumerator returnOrc() 
    {
        yield return new WaitForSeconds(6);
        // Debug.Log("Pool로 return");
        PoolingEnemyGenerator.Instance.ReturnToPool(this.gameObject);
    }

    private void Update()
    {
        // FSM의 Excute 실행 
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}

