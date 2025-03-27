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
        // Debug.Log("Pool�� return");
        PoolingEnemyGenerator.Instance.ReturnToPool(this.gameObject);
    }

    private void Update()
    {
        // FSM�� Excute ���� 
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}

