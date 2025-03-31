using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour, IAttack<BaseUnit>
{
    [SerializeField]
    private BaseUnit Owner;

    public void IAttackInit(BaseUnit temp)
    {
        this.Owner = temp;
    }

    public void IAttack_Enter()
    {
        // 히트 범위 만큼 overlapSpere 해서 안에 플레이어 검출하면 데미지주기

        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position , Owner.UnitState.hitRange);
    }

    public void IAttack()
    {
                
    }

    public void IAttack_Exit()
    {
                   
    }
}
