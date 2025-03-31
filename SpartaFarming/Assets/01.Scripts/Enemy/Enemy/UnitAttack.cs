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
        StartCoroutine(AttackCoru());
    }

    public void IAttack()
    {
        if (Owner.Player == null)
        {
            Debug.LogError("Base Unit�� Player�� null ���� ");
            return;
        }

        // �ø�
        Owner.Flip(Owner.Player.position.x);
        // ���
        Owner.IsDie();

        // attack ���� ������ -> Tracking
        if ( ! Owner.isInRange(Owner.UnitState.attackTriggerRange) )
        {
            Owner.ChageState(EnemyState.Tracking);
        }
    }

    public void IAttack_Exit()
    {
        StopAllCoroutines();  
    }

    IEnumerator AttackCoru() 
    {
        while (true) 
        {

            // ��Ʈ ���� ��ŭ overlapSpere , player ���̾
            Collider2D[] collider = Physics2D.OverlapCircleAll
                (transform.position, Owner.UnitState.hitRange, UnitManager.Instance.PlayerLayer);

            if (collider.Length != 0)
            {
                // ##TODO �÷��̾�� ������  

            }

            yield return new WaitForSeconds(Owner.UnitState.attackCoolTime);
        }
    }
}
