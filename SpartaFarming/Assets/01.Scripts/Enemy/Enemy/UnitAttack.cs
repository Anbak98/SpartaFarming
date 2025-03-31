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
        // 히트 범위 만큼 overlapSpere , player 레이어만
        Collider2D[] collider = Physics2D.OverlapCircleAll
            (transform.position , Owner.UnitState.hitRange, UnitManager.Instance.PlayerLayer);

        if (collider.Length != 0) 
        {
            // ##TODO 플레이어에게 데미지 
        }
    }

    public void IAttack()
    {
                
    }

    public void IAttack_Exit()
    {
                   
    }
}
