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
        // ��Ʈ ���� ��ŭ overlapSpere �ؼ� �ȿ� �÷��̾� �����ϸ� �������ֱ�

        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position , Owner.UnitState.hitRange);
    }

    public void IAttack()
    {
                
    }

    public void IAttack_Exit()
    {
                   
    }
}
